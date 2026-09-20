using System.ComponentModel.DataAnnotations;

namespace Inventario.Application.DTOs;

public class DisminuirInventarioDTO
{
    [Required]
    public List<ProductoCantidadDTO> Productos { get; set; }
}

public class ProductoCantidadDTO
{
    [Required]
    public Guid ProductoId { get; set; }
    [Required]
    public int Cantidad { get; set; }
}
