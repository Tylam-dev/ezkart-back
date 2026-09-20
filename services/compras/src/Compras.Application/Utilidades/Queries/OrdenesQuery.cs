using Compras.Domain.Enums;

namespace Compras.Application.Utilidades.Queries;

public class OrdenesQuery
{
    public EstadoOrdenEnum? EstadoOrden { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
}
