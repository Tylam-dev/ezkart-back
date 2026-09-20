
namespace Inventario.Domain.Exepcion;
public class ExepcionDominio : Exception
{
    public ExepcionDominio(string mensaje) : base(mensaje)
    {
    }
}