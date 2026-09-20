using System.ComponentModel.DataAnnotations;

namespace Compras.Application.DTOs;

public class AgregarProductoCarritoDTO
{
    [Required]
    public Guid ProductoId { get; set; }
    [Required]
    public int Cantidad { get; set; }
}
