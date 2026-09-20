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
