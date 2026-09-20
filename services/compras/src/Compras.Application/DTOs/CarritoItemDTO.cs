namespace Compras.Application.DTOs;

public class CarritoItemDTO
{
    public Guid ProductoId { get; set; }
    public string Codigo { get; set; } = null!;
    public int Cantidad { get; set; }
}
