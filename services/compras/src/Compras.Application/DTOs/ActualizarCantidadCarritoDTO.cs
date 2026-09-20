using System.ComponentModel.DataAnnotations;

namespace Compras.Application.DTOs;

public class ActualizarCantidadCarritoDTO
{
    [Required]
    public string Codigo { get; set; } = null!;
    [Required]
    public int Cantidad { get; set; }
}
