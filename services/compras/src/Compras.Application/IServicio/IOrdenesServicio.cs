using Compras.Application.DTOs;
using Compras.Application.Utilidades;
using Compras.Application.Utilidades.Queries;

public interface IOrdenesServicio
{
    Task<Paginacion<OrdenDTO>> ObtenerOrdenesPaginado(
        Guid usuarioId,
        PaginacionQuery<OrdenesQuery> query
    );
}
