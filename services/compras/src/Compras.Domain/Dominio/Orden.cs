using Compras.Domain.Enums;

namespace Compras.Domain.Dominio;

public class Orden
{
    public Guid Id {get;set;}
    public Guid UsuarioId {get;set;}
    public DateTime FechaCreacion {get;set;}
    public EstadoOrdenEnum EstadoOrden {get;set;}
    public DescuentoTemporada? DescuentoTemporada {get;set;}
    public List<OrdenDetalle> Detalles {get;set;} = new List<OrdenDetalle>();

    public decimal Subtotal => Detalles.Sum(d => d.Subtotal);

    public decimal MontoDescuento => DescuentoTemporada is null
        ? 0
        : Math.Round(Subtotal * DescuentoTemporada.Porcentaje / 100m, 2, MidpointRounding.AwayFromZero);

    public decimal Total => Subtotal - MontoDescuento;
}
