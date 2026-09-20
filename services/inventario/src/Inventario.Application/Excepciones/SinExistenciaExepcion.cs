namespace Inventario.Application.Exepciones;
public class SinExistenciaExepcion : Exception
{
    public SinExistenciaExepcion() : base("No existen mas productos")
    {
    }
}