namespace Compras.Domain.Dominio;

public class DescuentoTemporada
{
    public Guid Id {get;set;}
    public string Nombre {get;set;} = null!;
    public decimal Porcentaje {get;set;}
    public DateTime FechaDesde {get;set;}
    public DateTime FechaHasta {get;set;}
    public DateTime FechaCreacion {get;set;}

    public bool EstaVigente(DateTime fecha) => fecha >= FechaDesde && fecha <= FechaHasta;
}
