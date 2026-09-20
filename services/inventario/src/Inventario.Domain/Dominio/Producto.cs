using Inventario.Domain.Exepcion;

namespace Inventario.Domain.Dominio;

public class Producto
{
    public Guid Id {get;set;}
    public string Codigo {get;set;}
    public string Nombre {get;set;}
    public decimal Precio {get;set;}
    public int Existencia {get;set;}
    public uint Version {get;set;}
    public bool TieneStock(int cantidad) => cantidad > 0 && Existencia >= cantidad;

    public void Descontar(int cantidad)
    {
        if (cantidad <= 0)
            throw new ExepcionDominio("La cantidad debe ser mayor que cero.");
        if (Existencia < cantidad)
            throw new ExepcionStockInsuficiente();

        Existencia -= cantidad;
    }

    public void Reponer(int cantidad)
    {
        if (cantidad <= 0)
            throw new ExepcionDominio("La cantidad debe ser mayor que cero.");
        Existencia += cantidad;
    }
}