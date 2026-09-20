namespace Compras.Domain.Dominio;

public class CarritoItem
{
    public Guid ProductoId {get;set;}
    public string Codigo {get;set;} = null!;
    public int Cantidad {get;set;}
}
