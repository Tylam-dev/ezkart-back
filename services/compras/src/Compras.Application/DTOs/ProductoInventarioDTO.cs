namespace Compras.Application.DTOs;

public class ProductoInventarioDTO
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public decimal Precio { get; set; }
    public int Existencia { get; set; }
}
