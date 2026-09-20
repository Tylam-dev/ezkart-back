using Compras.Domain.Exepcion;

namespace Compras.Domain.Dominio;

public class Carrito
{
    public Guid Id {get;set;}
    public Guid UsuarioId {get;set;}
    public List<CarritoItem> Items {get;set;} = new List<CarritoItem>();

    public int ObtenerCantidadProducto(Guid productoId) =>
        Items.FirstOrDefault(i => i.ProductoId == productoId)?.Cantidad ?? 0;

    public void AgregarProducto(Guid productoId, string codigo, string nombre, int cantidad)
    {
        if (cantidad <= 0)
            throw new ExepcionDominio("La cantidad debe ser mayor que cero.");

        var item = Items.FirstOrDefault(i => i.ProductoId == productoId);

        if (item is null)
            Items.Add(new CarritoItem() { ProductoId = productoId, Codigo = codigo, Nombre = nombre, Cantidad = cantidad });
        else
        {
            item.Cantidad += cantidad;
            item.Nombre = nombre;
        }
    }

    public void ActualizarCantidad(Guid productoId, int cantidad)
    {
        if (cantidad <= 0)
            throw new ExepcionDominio("La cantidad debe ser mayor que cero.");

        var item = Items.FirstOrDefault(i => i.ProductoId == productoId)
            ?? throw new ExepcionProductoNoEnCarrito();

        item.Cantidad = cantidad;
    }

    public void EliminarProducto(Guid productoId)
    {
        var item = Items.FirstOrDefault(i => i.ProductoId == productoId)
            ?? throw new ExepcionProductoNoEnCarrito();

        Items.Remove(item);
    }
}
