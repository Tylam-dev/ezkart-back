namespace Inventario.Application.Exepciones;
public class ExepcionProductoNoEncontrado : Exception
{
    public ExepcionProductoNoEncontrado() : base("Producto no encontrado")
    {
    }
}
