using System.Security.Claims;
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
            if (!ObtenerUsuarioId(out var usuarioId)) return Unauthorized();

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
            if (!ObtenerUsuarioId(out var usuarioId)) return Unauthorized();

            return Ok();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
    [HttpPut("items/{productId:Guid}")]
    public async Task<IActionResult> ActualizarCantidadProducto([FromRoute] Guid productId,
        [FromBody] ActualizarCantidadCarritoDTO actualizarCantidadCarritoDTO
    )
    {
        try
        {
            if (!ObtenerUsuarioId(out var usuarioId)) return Unauthorized();

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
            if (!ObtenerUsuarioId(out var usuarioId)) return Unauthorized();

            return NoContent();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
    private bool ObtenerUsuarioId(out Guid usuarioId)
    {
        var claimUsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(claimUsuarioId, out usuarioId);
    }
}
