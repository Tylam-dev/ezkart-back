namespace Inventario.Application.Exepciones;
public class ExepcionConcurrencia : Exception
{
    public ExepcionConcurrencia() : base("El inventario fue modificado por otro proceso")
    {
    }
}
