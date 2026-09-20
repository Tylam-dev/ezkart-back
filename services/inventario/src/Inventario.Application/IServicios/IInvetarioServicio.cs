using Inventario.Application.DTOs;
using Inventario.Application.Utilidades;
using Inventario.Application.Utilidades.Queries;
using Inventario.Domain.Dominio;

namespace Invetario.Appliccation.IServcio;

public interface IInvetarioServicio
{
    Task<Paginacion<Producto>> ObtenerProductosFiltradosPaginado(
        PaginacionQuery<ProductosQuery> query
    );
    Task<Producto?> ObtenerProductoPorId(Guid productoId);
    Task<bool> DisminuirInventario(DisminuirInventarioDTO disminuirInventarioDTO);
}