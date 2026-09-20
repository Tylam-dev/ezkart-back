using System.ComponentModel.DataAnnotations;

namespace Compras.Application.DTOs;

public class CarritoItemDTO
{
    [Required]
    public Guid ProductoId { get; set; }
    [Required]
    public string Codigo { get; set; } = null!;
    [Required]
    public int Cantidad { get; set; }
}
