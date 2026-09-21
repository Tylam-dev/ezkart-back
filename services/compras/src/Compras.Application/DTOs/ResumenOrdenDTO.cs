namespace Compras.Application.DTOs;

public class ResumenOrdenDTO
{
    public List<OrdenDetalleDTO> Detalles { get; set; } = new List<OrdenDetalleDTO>();
    public decimal Subtotal { get; set; }
    public Guid? DescuentoTemporadaId { get; set; }
    public string? NombreDescuento { get; set; }
    public decimal PorcentajeDescuento { get; set; }
    public decimal MontoDescuento { get; set; }
    public decimal Total { get; set; }
}

public class OrdenDetalleDTO
{
    public Guid ProductoId { get; set; }
    public string Codigo { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public decimal PrecioUnitario { get; set; }
    public int Cantidad { get; set; }
    public decimal Subtotal { get; set; }
}
