using Inventario.Application.Utilidades;
using Inventario.Application.Utilidades.Queries;
using Inventario.Domain.Dominio;

namespace Inventario.Application;

public interface IRepositorioInventario
{
    Task<Resultado<Paginacion<Producto>>> ObtenerProductosFiltradosPaginado(
        PaginacionQuery<ProductosQuery> queryProductos
    );
    Task<Resultado<Producto?>> ObtenerProductoPorId(Guid productoId);
    Task<Resultado<Producto?>> ObtenerProductoPorIdYCodigo(Guid productoId, string codigo);
    Task<Resultado<List<Producto>>> ObtenerListaProductoPorId(List<Guid> productoIds);
    Task<Resultado<bool>> ActualizarExistencias(List<Producto> productos);
}
