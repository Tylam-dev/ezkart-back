namespace Compras.Domain.Exepcion;
public class ExepcionProductoNoEnCarrito : Exception
{
    public ExepcionProductoNoEnCarrito() : base("El producto no esta en el carrito")
    {
    }
}
