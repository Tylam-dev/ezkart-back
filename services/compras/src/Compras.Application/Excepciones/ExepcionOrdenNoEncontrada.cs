namespace Compras.Application.Exepciones;
public class ExepcionOrdenNoEncontrada : Exception
{
    public ExepcionOrdenNoEncontrada() : base("Orden no encontrada")
    {
    }
}
