namespace Compras.Domain.Dominio;

public class OrdenDetalle
{
    public Guid ProductoId {get;set;}
    public string Codigo {get;set;} = null!;
    public string Nombre {get;set;} = null!;
    public decimal PrecioUnitario {get;set;}
    public int Cantidad {get;set;}
    public decimal Subtotal => PrecioUnitario * Cantidad;
}
