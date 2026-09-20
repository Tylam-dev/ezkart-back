using Compras.Application;
using Compras.Application.DTOs;
using Compras.Application.Exepciones;
using Compras.Domain.Dominio;
using Compras.Domain.Exepcion;

public class CarritoServicio : ICarritoServicio
{
    private readonly IRepositorioCarrito _repositorioCarrito;
    private readonly IClienteHttpInventario _clienteHttpInventario;
    public CarritoServicio(
        IRepositorioCarrito repositorioCarrito,
        IClienteHttpInventario clienteInventario
    )
    {
        _repositorioCarrito = repositorioCarrito;
        _clienteHttpInventario = clienteInventario;
    }
    public async Task<CarritoDTO> ObtenerCarrito(Guid usuarioId)
    {
        var carrito = await ObtenerCarritoUsuario(usuarioId);

        if(carrito is null) return new CarritoDTO();

        return MapearCarritoADTO(carrito);
    }
    public async Task<CarritoDTO> AgregarProducto(Guid usuarioId, AgregarProductoCarritoDTO agregarProductoCarritoDTO)
    {
        if(agregarProductoCarritoDTO.Cantidad <= 0)
            throw new ExepcionDominio("La cantidad debe ser mayor que cero.");

        var articulo = await _clienteHttpInventario.BuscarProducto(
            agregarProductoCarritoDTO.ProductoId,
            agregarProductoCarritoDTO.Codigo
        );

        if(!articulo.Exitoso) throw articulo.Excepcion!;

        var carrito = await ObtenerCarritoUsuario(usuarioId);

        if(carrito is null) carrito = new Carrito() { Id = Guid.NewGuid(), UsuarioId = usuarioId };

        var cantidadTotal = carrito.ObtenerCantidadProducto(agregarProductoCarritoDTO.ProductoId) + agregarProductoCarritoDTO.Cantidad;

        if(cantidadTotal > articulo.Valor.Existencia) throw new ExepcionStockInsuficiente();

        carrito.AgregarProducto(
            agregarProductoCarritoDTO.ProductoId,
            agregarProductoCarritoDTO.Codigo,
            agregarProductoCarritoDTO.Cantidad
        );

        await GuardarCarrito(carrito);

        return MapearCarritoADTO(carrito);
    }
    public async Task<CarritoDTO> ActualizarCantidadProducto(Guid usuarioId, Guid productoId,
        ActualizarCantidadCarritoDTO actualizarCantidadCarritoDTO
    )
    {
        if(actualizarCantidadCarritoDTO.Cantidad <= 0)
            throw new ExepcionDominio("La cantidad debe ser mayor que cero.");

        var carrito = await ObtenerCarritoUsuario(usuarioId) ?? throw new ExepcionProductoNoEnCarrito();

        if(carrito.ObtenerCantidadProducto(productoId) == 0) throw new ExepcionProductoNoEnCarrito();

        var articulo = await _clienteHttpInventario.BuscarProducto(productoId, actualizarCantidadCarritoDTO.Codigo);

        if(!articulo.Exitoso) throw articulo.Excepcion!;

        if(actualizarCantidadCarritoDTO.Cantidad > articulo.Valor.Existencia) throw new ExepcionStockInsuficiente();

        carrito.ActualizarCantidad(productoId, actualizarCantidadCarritoDTO.Cantidad);

        await GuardarCarrito(carrito);

        return MapearCarritoADTO(carrito);
    }
    public async Task<bool> EliminarProducto(Guid usuarioId, Guid productoId)
    {
        var carrito = await ObtenerCarritoUsuario(usuarioId) ?? throw new ExepcionProductoNoEnCarrito();

        carrito.EliminarProducto(productoId);

        await GuardarCarrito(carrito);

        return true;
    }
    private async Task<Carrito?> ObtenerCarritoUsuario(Guid usuarioId)
    {
        var carrito = await _repositorioCarrito.ObtenerCarritoPorUsuario(usuarioId);

        if(!carrito.Exitoso) throw carrito.Excepcion!;

        return carrito.Valor;
    }
    private async Task GuardarCarrito(Carrito carrito)
    {
        var guardado = await _repositorioCarrito.GuardarCarrito(carrito);

        if(!guardado.Exitoso) throw guardado.Excepcion!;
    }
    private static CarritoDTO MapearCarritoADTO(Carrito carrito)
    {
        var carritoDTO = new CarritoDTO();

        foreach(var item in carrito.Items) carritoDTO.Items.Add(new CarritoItemDTO()
        {
            ProductoId = item.ProductoId,
            Codigo = item.Codigo,
            Cantidad = item.Cantidad
        });

        return carritoDTO;
    }
}
