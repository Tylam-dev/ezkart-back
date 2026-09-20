using Compras.Application.DTOs;
using Compras.Application.Utilidades;
using Compras.Application.Utilidades.Queries;
using Compras.Domain.Dominio;

public interface IOrdenesServicio
{
    Task<Orden> FinalizarCompra(Guid usuarioId);
    Task<Paginacion<OrdenDTO>> ObtenerOrdenesPaginado(
        Guid usuarioId,
        PaginacionQuery<OrdenesQuery> query
    );
    Task<Orden> ObtenerOrdenPorId(Guid usuarioId, Guid ordenId);
}
