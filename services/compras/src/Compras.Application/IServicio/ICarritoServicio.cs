using Compras.Application.DTOs;

public interface ICarritoServicio
{
    Task<CarritoDTO> ObtenerCarrito(Guid usuarioId);
    Task<CarritoDTO> AgregarProducto(Guid usuarioId, AgregarProductoCarritoDTO agregarProductoCarritoDTO);
    Task<CarritoDTO> ActualizarCantidadProducto(Guid usuarioId, Guid productoId,
        ActualizarCantidadCarritoDTO actualizarCantidadCarritoDTO
    );
    Task<bool> EliminarProducto(Guid usuarioId, Guid productoId);
}
