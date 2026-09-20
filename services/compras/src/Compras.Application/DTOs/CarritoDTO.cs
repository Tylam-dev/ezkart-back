namespace Compras.Application.DTOs;

public class CarritoDTO
{
    public List<CarritoItemDTO> Items { get; set; } = new List<CarritoItemDTO>();
}
