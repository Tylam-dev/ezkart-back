using Inventario.Application.Utilidades.Queries;
using Invetario.Appliccation.IServcio;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.Api.Controllers;

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
        catch (System.Exception)
        {
            // return Err
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
