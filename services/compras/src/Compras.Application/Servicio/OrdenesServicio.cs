using Compras.Application;
using Compras.Application.DTOs;
using Compras.Application.Exepciones;
using Compras.Application.Utilidades;
using Compras.Application.Utilidades.Queries;
using Compras.Domain.Dominio;
using Compras.Domain.Exepcion;

public class OrdenesServicio : IOrdenesServicio
{
    private readonly IRepositorioCompras _repositorioCompras;
    private readonly IRepositorioCarrito _repositorioCarrito;
    private readonly IClienteHttpInventario _clienteHttpInventario;
    public OrdenesServicio(
        IRepositorioCompras repositorioCompras,
        IRepositorioCarrito repositorioCarrito,
        IClienteHttpInventario clienteHttpInventario
    )
    {
        _repositorioCompras = repositorioCompras;
        _repositorioCarrito = repositorioCarrito;
        _clienteHttpInventario = clienteHttpInventario;
    }
    public async Task<Orden> FinalizarCompra(Guid usuarioId)
    {
        var carrito = await ObtenerCarritoUsuario(usuarioId);

        if(carrito is null || carrito.Items.Count == 0)
            throw new ExepcionDominio("No se puede hacer una compra con un carrito vacio");

        var ordenBuilder = new OrdenBuilder(usuarioId);
        var disminuirInventarioDTO = new DisminuirInventarioDTO();

        foreach(var item in carrito.Items)
        {
            var articulo = await _clienteHttpInventario.BuscarProducto(item.ProductoId, item.Codigo);

            if(!articulo.Exitoso) throw articulo.Excepcion!;

            ordenBuilder.AgregarProducto(
                item.ProductoId,
                item.Codigo,
                articulo.Valor.Nombre,
                articulo.Valor.Precio,
                item.Cantidad
            );

            disminuirInventarioDTO.Productos.Add(new ProductoCantidadDTO()
            {
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad
            });
        }

        var descuentos = await _repositorioCompras.ObtenerDescuentosVigentes(DateTime.UtcNow);

        if(!descuentos.Exitoso) throw descuentos.Excepcion!;

        var descuento = descuentos.Valor.OrderByDescending(d => d.FechaCreacion).FirstOrDefault();

        if(descuento is not null) ordenBuilder.AplicarDescuento(descuento);

        var orden = ordenBuilder.Construir();

        var disminuido = await _clienteHttpInventario.DisminuirInventario(disminuirInventarioDTO);

        if(!disminuido.Exitoso) throw disminuido.Excepcion!;

        var guardada = await _repositorioCompras.GuardarOrden(orden);

        if(!guardada.Exitoso) throw guardada.Excepcion!;

        return orden;
    }
    public async Task<Paginacion<OrdenDTO>> ObtenerOrdenesPaginado(
        Guid usuarioId,
        PaginacionQuery<OrdenesQuery> query
    )
    {
        var ordenes = await _repositorioCompras.ObtenerOrdenesPaginado(usuarioId, query);

        if(!ordenes.Exitoso) throw ordenes.Excepcion!;

        return MapearOrdenesADTO(ordenes.Valor!);
    }
    public async Task<Orden> ObtenerOrdenPorId(Guid usuarioId, Guid ordenId)
    {
        var orden = await _repositorioCompras.ObtenerOrdenPorId(usuarioId, ordenId);

        if(!orden.Exitoso) throw orden.Excepcion!;

        if(orden.Valor is null) throw new ExepcionOrdenNoEncontrada();

        return orden.Valor;
    }
    private async Task<Carrito?> ObtenerCarritoUsuario(Guid usuarioId)
    {
        var carrito = await _repositorioCarrito.ObtenerCarritoPorUsuario(usuarioId);

        if(!carrito.Exitoso) throw carrito.Excepcion!;

        return carrito.Valor;
    }
    private static Paginacion<OrdenDTO> MapearOrdenesADTO(Paginacion<Orden> ordenes)
    {
        var ordenesDTO = new List<OrdenDTO>();

        foreach(var orden in ordenes.Items) ordenesDTO.Add(new OrdenDTO()
        {
            Id = orden.Id,
            FechaCreacion = orden.FechaCreacion,
            EstadoOrden = orden.EstadoOrden,
            Total = orden.Total,
            DescuentoTemporadaId = orden.DescuentoTemporada?.Id
        });

        return new Paginacion<OrdenDTO>()
        {
            Items = ordenesDTO,
            Pagina = ordenes.Pagina,
            TamanoPagina = ordenes.TamanoPagina,
            TotalItems = ordenes.TotalItems,
            TotalPaginas = ordenes.TotalPaginas
        };
    }
}
