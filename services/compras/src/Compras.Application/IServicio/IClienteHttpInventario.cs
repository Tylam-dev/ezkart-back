using Compras.Application.DTOs;
using Compras.Application.Utilidades;

public interface IClienteHttpInventario
{
    Task<Resultado<ProductoInventarioDTO>> BuscarProducto(Guid productoId, string codigo);
    Task<Resultado<bool>> DisminuirInventario(DisminuirInventarioDTO disminuirInventarioDTO);
}
