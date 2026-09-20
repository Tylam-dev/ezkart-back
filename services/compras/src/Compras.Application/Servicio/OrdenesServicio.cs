using Compras.Application.DTOs;
using Compras.Application.Utilidades;
using Compras.Application.Utilidades.Queries;

public class OrdenesServicio : IOrdenesServicio
{
    public Task<Paginacion<OrdenDTO>> ObtenerOrdenesPaginado(
        Guid usuarioId,
        PaginacionQuery<OrdenesQuery> query
    )
    {
        throw new NotImplementedException();
    }
}
