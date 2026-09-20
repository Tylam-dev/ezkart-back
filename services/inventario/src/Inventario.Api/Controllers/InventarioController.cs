using Inventario.Application.DTOs;
using Inventario.Application.Exepciones;
using Inventario.Application.Utilidades.Queries;
using Inventario.Domain.Exepcion;
using Invetario.Appliccation.IServcio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.Api.Controllers;

[Authorize]
[ApiController]
[Route("products")]
public class InventarioController : ControllerBase
{
    private readonly ILogger<InventarioController> _logger;
    private readonly IInvetarioServicio _inventarioServicio;
    public InventarioController(
        ILogger<InventarioController> logger,
        IInvetarioServicio invetarioServicio
    )
    {
        _logger = logger;
        _inventarioServicio = invetarioServicio;
    }
    [HttpGet]
    public async Task<IActionResult> ObtenerProductos(
        [FromQuery] PaginacionQuery<ProductosQuery> queryPaginada
    )
    {
        try
        {
            var listaProductos = await _inventarioServicio.ObtenerProductosFiltradosPaginado(queryPaginada);

            _logger.LogInformation("Productos entregados");

            return Ok(listaProductos);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> ObtenerUnicoProducto([FromRoute] Guid id)
    {
        try
        {
            var producto = await _inventarioServicio.ObtenerProductoPorId(id);

            if (producto is null) return NotFound(new { Message = "Producto no encontrado" });

            _logger.LogInformation("Producto entregado");

            return Ok(producto);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
    [HttpGet("buscar")]
    public async Task<IActionResult> BuscarProducto([FromQuery] Guid productoId, [FromQuery] string codigo)
    {
        try
        {
            var producto = await _inventarioServicio.ObtenerProductoPorIdYCodigo(productoId, codigo);

            _logger.LogInformation("Producto encontrado");

            return Ok(producto);
        }
        catch (ExepcionProductoNoEncontrado ex)
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
    [HttpPost("disminuir")]
    public async Task<IActionResult> DisminuirInventario([FromBody] DisminuirInventarioDTO disminuirInventarioDTO)
    {
        try
        {
            await _inventarioServicio.DisminuirInventario(disminuirInventarioDTO);

            _logger.LogInformation("Inventario disminuido");

            return Ok();
        }
        catch (ExepcionDominio ex)
        {
            _logger.LogWarning(ex.Message);
            return BadRequest(new { Message = ex.Message });
        }
        catch (ExepcionProductoNoEncontrado ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(new { Message = ex.Message });
        }
        catch (ExepcionStockInsuficiente ex)
        {
            _logger.LogWarning(ex.Message);
            return Conflict(new { Message = ex.Message });
        }
        catch (ExepcionConcurrencia ex)
        {
            _logger.LogWarning(ex.Message);
            return Conflict(new { Message = ex.Message });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
}
