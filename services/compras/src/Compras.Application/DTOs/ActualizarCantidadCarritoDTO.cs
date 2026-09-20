using System.ComponentModel.DataAnnotations;

namespace Compras.Application.DTOs;

public class ActualizarCantidadCarritoDTO
{
    [Required]
    public int Cantidad { get; set; }
}
