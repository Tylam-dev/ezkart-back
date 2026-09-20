using System.Security.Claims;
using Compras.Application.DTOs;
using Compras.Application.Exepciones;
using Compras.Domain.Exepcion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Compras.Api.Controllers;

[Authorize]
[ApiController]
[Route("cart")]
public class CarritoController : ControllerBase
{
    private readonly ILogger<CarritoController> _logger;
    private readonly ICarritoServicio _carritoServicio;
    public CarritoController(
        ILogger<CarritoController> logger,
        ICarritoServicio carritoServicio
    )
    {
        _logger = logger;
        _carritoServicio = carritoServicio;
    }
    [HttpGet]
    public async Task<IActionResult> ObtenerCarrito()
    {
        try
        {
            if (!ObtenerUsuarioId(out var usuarioId)) return Unauthorized();

            var carrito = await _carritoServicio.ObtenerCarrito(usuarioId);

            _logger.LogInformation("Carrito entregado");

            return Ok(carrito);
        }
        catch (ExepcionInventarioNoDisponible ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(503, new { Message = ex.Message });
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

            var carrito = await _carritoServicio.AgregarProducto(usuarioId, agregarProductoCarritoDTO);

            _logger.LogInformation("Producto agregado al carrito");

            return Ok(carrito);
        }
        catch (ExepcionDominio ex)
        {
            _logger.LogWarning(ex.Message);
            return BadRequest(new { Message = ex.Message });
        }
        catch (ExepcionArticuloNoEncontrado ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(new { Message = ex.Message });
        }
        catch (ExepcionStockInsuficiente ex)
        {
            _logger.LogWarning(ex.Message);
            return Conflict(new { Message = ex.Message });
        }
        catch (ExepcionInventarioNoDisponible ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(503, new { Message = ex.Message });
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

            var carrito = await _carritoServicio.ActualizarCantidadProducto(usuarioId, productId, actualizarCantidadCarritoDTO);

            _logger.LogInformation("Cantidad del carrito actualizada");

            return Ok(carrito);
        }
        catch (ExepcionDominio ex)
        {
            _logger.LogWarning(ex.Message);
            return BadRequest(new { Message = ex.Message });
        }
        catch (ExepcionProductoNoEnCarrito ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(new { Message = ex.Message });
        }
        catch (ExepcionArticuloNoEncontrado ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(new { Message = ex.Message });
        }
        catch (ExepcionStockInsuficiente ex)
        {
            _logger.LogWarning(ex.Message);
            return Conflict(new { Message = ex.Message });
        }
        catch (ExepcionInventarioNoDisponible ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(503, new { Message = ex.Message });
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

            await _carritoServicio.EliminarProducto(usuarioId, productId);

            _logger.LogInformation("Producto eliminado del carrito");

            return NoContent();
        }
        catch (ExepcionProductoNoEnCarrito ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(new { Message = ex.Message });
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
