using Inventario.Application.Utilidades;
using Inventario.Application.Utilidades.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.Api.Controllers;

[ApiController]
[Route("products")]
public class InventarioController : ControllerBase
{
    private readonly ILogger<InventarioController> _logger;

    public InventarioController(
        ILogger<InventarioController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> ObtenerProductos(
        [FromQuery] PaginacionQuery<ProductosQuery> queryPaginada
    )
    {
        try
        {
            
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> ObtenerUnicoProducto([FromQuery] Guid id)
    {
        try
        {
            
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
}
