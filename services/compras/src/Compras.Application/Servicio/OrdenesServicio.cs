using Compras.Application.DTOs;
using Compras.Application.Utilidades;
using Compras.Application.Utilidades.Queries;
using Compras.Domain.Dominio;

public class OrdenesServicio : IOrdenesServicio
{
    public Task<Orden> FinalizarCompra(Guid usuarioId)
    {
    }
    public Task<Paginacion<OrdenDTO>> ObtenerOrdenesPaginado(
        Guid usuarioId,
        PaginacionQuery<OrdenesQuery> query
    )
    {
    }
    public Task<Orden> ObtenerOrdenPorId(Guid usuarioId, Guid ordenId)
    {
    }
}
