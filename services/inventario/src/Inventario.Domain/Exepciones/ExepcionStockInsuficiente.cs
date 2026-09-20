
namespace Inventario.Domain.Exepcion;
public class ExepcionStockInsuficiente : Exception
{
    public ExepcionStockInsuficiente() : base("Stock Agotado")
    {
    }
}