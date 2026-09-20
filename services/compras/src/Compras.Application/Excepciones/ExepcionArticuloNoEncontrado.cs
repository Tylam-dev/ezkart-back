namespace Compras.Application.Exepciones;
public class ExepcionArticuloNoEncontrado : Exception
{
    public ExepcionArticuloNoEncontrado() : base("Articulo no encontrado")
    {
    }
}
