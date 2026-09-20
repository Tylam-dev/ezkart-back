using System.Security.Claims;
using Compras.Application.Utilidades.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Compras.Api.Controllers;

[Authorize]
[ApiController]
[Route("orders")]
public class OrdenesController : ControllerBase
{
    private readonly ILogger<OrdenesController> _logger;
    private readonly IOrdenesServicio _ordenesServicio;
    public OrdenesController(
        ILogger<OrdenesController> logger,
        IOrdenesServicio ordenesServicio
    )
    {
        _logger = logger;
        _ordenesServicio = ordenesServicio;
    }
    [HttpPost]
    public async Task<IActionResult> FinalizarCompra()
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
    [HttpGet]
    public async Task<IActionResult> ObtenerOrdenes(
        [FromQuery] PaginacionQuery<OrdenesQuery> queryPaginada
    )
    {
        try
        {
            if (!ObtenerUsuarioId(out var usuarioId)) return Unauthorized();

            var listaOrdenes = await _ordenesServicio.ObtenerOrdenesPaginado(usuarioId, queryPaginada);

            _logger.LogInformation("Ordenes entregadas");

            return Ok(listaOrdenes);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> ObtenerUnicaOrden([FromRoute] Guid id)
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
    private bool ObtenerUsuarioId(out Guid usuarioId)
    {
        var claimUsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(claimUsuarioId, out usuarioId);
    }
}
