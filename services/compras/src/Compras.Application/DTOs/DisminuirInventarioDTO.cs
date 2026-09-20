namespace Compras.Application.DTOs;

public class DisminuirInventarioDTO
{
    public List<ProductoCantidadDTO> Productos { get; set; } = new List<ProductoCantidadDTO>();
}

public class ProductoCantidadDTO
{
    public Guid ProductoId { get; set; }
    public int Cantidad { get; set; }
}
