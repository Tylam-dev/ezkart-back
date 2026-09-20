namespace Compras.Application.Exepciones;
public class ExepcionInventarioNoDisponible : Exception
{
    public ExepcionInventarioNoDisponible(Exception? excepcionInterna = null)
        : base("Inventario no disponible en este momento", excepcionInterna)
    {
    }
}
