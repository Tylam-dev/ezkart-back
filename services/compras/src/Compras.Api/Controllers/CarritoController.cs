using Compras.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Compras.Api.Controllers;

[Authorize]
[ApiController]
[Route("cart")]
public class CarritoController : ControllerBase
{
    private readonly ILogger<CarritoController> _logger;
    public CarritoController(
        ILogger<CarritoController> logger
    )
    {
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> ObtenerCarrito()
    {
        try
        {
            return Ok();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
    [HttpPost("items")]
    public async Task<IActionResult> AgregarProducto([FromBody] AgregarProductoCarritoDTO agregarProductoCarritoDTO)
    {
        try
        {
            return Ok();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
    [HttpPut("items/{productId:Guid}")]
    public async Task<IActionResult> ActualizarCantidadProducto(
        [FromRoute] Guid productId,
        [FromBody] ActualizarCantidadCarritoDTO actualizarCantidadCarritoDTO
    )
    {
        try
        {
            return Ok();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
    [HttpDelete("items/{productId:Guid}")]
    public async Task<IActionResult> EliminarProducto([FromRoute] Guid productId)
    {
        try
        {
            return NoContent();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
}
