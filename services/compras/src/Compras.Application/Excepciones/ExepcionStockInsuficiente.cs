namespace Compras.Application.Exepciones;
public class ExepcionStockInsuficiente : Exception
{
    public ExepcionStockInsuficiente() : base("No hay existencia suficiente del articulo")
    {
    }
    public ExepcionStockInsuficiente(string mensaje) : base(mensaje)
    {
    }
}
