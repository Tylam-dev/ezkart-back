using Compras.Domain.Enums;

namespace Compras.Application.DTOs;

public class OrdenDTO
{
    public Guid Id { get; set; }
    public DateTime FechaCreacion { get; set; }
    public EstadoOrdenEnum EstadoOrden { get; set; }
    public decimal Total { get; set; }
    public Guid? DescuentoTemporadaId { get; set; }
    public List<OrdenDetalleDTO> Detalles { get; set; } = new List<OrdenDetalleDTO>();
    public decimal Subtotal { get; set; }
    public decimal MontoDescuento { get; set; }
}
