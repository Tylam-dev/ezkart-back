using Compras.Application.Utilidades;
using Compras.Application.Utilidades.Queries;
using Compras.Domain.Dominio;

namespace Compras.Application;

public interface IRepositorioCompras
{
    Task<Resultado<Paginacion<Orden>>> ObtenerOrdenesPaginado(
        Guid usuarioId,
        PaginacionQuery<OrdenesQuery> queryOrdenes
    );
    Task<Resultado<Orden?>> ObtenerOrdenPorId(Guid usuarioId, Guid ordenId);
    Task<Resultado<bool>> GuardarOrden(Orden orden);
    Task<Resultado<List<DescuentoTemporada>>> ObtenerDescuentosVigentes(DateTime fecha);
    Task<Resultado<DescuentoTemporada?>> ObtenerDescuentoPorId(Guid descuentoId);
}
