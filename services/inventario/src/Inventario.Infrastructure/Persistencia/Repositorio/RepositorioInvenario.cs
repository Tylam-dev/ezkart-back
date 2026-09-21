using Inventario.Application;
using Inventario.Application.Utilidades;
using Inventario.Application.Utilidades.Queries;
using Inventario.Domain.Dominio;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistencia.Repositorio;

internal class RepositorioInvenario : IRepositorioInventario
{
    private readonly InventarioDBContext _context;

    public RepositorioInvenario(InventarioDBContext context)
    {
        _context = context;
    }

    public async Task<Resultado<Paginacion<Producto>>> ObtenerProductosFiltradosPaginado(
        PaginacionQuery<ProductosQuery> queryProductos
    )
    {
        try
        {
            var query = _context.Producto
                .Where(p => p.Estado == (char)Domain.Enums.EstadoEnum.Activo);

            var totalProductos = await query.CountAsync();

            var productos = await query
                .Where(p => p.Estado == (char)Domain.Enums.EstadoEnum.Activo)
                .Skip(queryProductos.Saltos)
                .Take(queryProductos.TamanoPagina)
                .ToListAsync();

            var totalPaginas = (int)Math.Ceiling(
                totalProductos / (double)queryProductos.TamanoPagina);

            var listaProductos = new List<Producto>(){};

            foreach( var producto in productos) listaProductos.Add(new Producto()
            {
                Id = producto.Id,
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Existencia = producto.Existencia,
                Version = producto.Version
            });

            var informacionPaginada =  new Paginacion<Producto>()
            {
                Items = listaProductos,
                Pagina = queryProductos.Pagina,
                TamanoPagina = queryProductos.TamanoPagina,
                TotalItems = totalProductos,
                TotalPaginas = totalPaginas

            };
            return Resultado<Paginacion<Producto>>.Exito(informacionPaginada);
        }
        catch (System.Exception ex)
        {
            return Resultado<Paginacion<Producto>>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<Producto?>> ObtenerProductoPorId(Guid productoId)
    {
        try
        {
            var resultado = Resultado<Producto?>.Exito(null);

            var producto = await _context.Producto
                .Where(p => p.Id == productoId &&
                            p.Estado == (char)Domain.Enums.EstadoEnum.Activo)
                .FirstOrDefaultAsync();
            
            if(producto is null) return resultado;

            var productoMapeado = new Producto()
            {
                Id = producto.Id,
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Existencia = producto.Existencia,
                Version = producto.Version
            };
            resultado.Valor = productoMapeado;

            return resultado;
        }
        catch (System.Exception ex)
        {
            return Resultado<Producto?>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<Producto?>> ObtenerProductoPorIdYCodigo(Guid productoId, string codigo)
    {
        try
        {
            var resultado = Resultado<Producto?>.Exito(null);

            var producto = await _context.Producto
                .Where(p => p.Id == productoId &&
                            p.Codigo == codigo &&
                            p.Estado == (char)Domain.Enums.EstadoEnum.Activo)
                .FirstOrDefaultAsync();

            if(producto is null) return resultado;

            var productoMapeado = new Producto()
            {
                Id = producto.Id,
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Existencia = producto.Existencia,
                Version = producto.Version
            };
            resultado.Valor = productoMapeado;

            return resultado;
        }
        catch (System.Exception ex)
        {
            return Resultado<Producto?>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<List<Producto>>> ObtenerListaProductoPorId(List<Guid> productoIds)
    {
        try
        {
            var resultado = Resultado<List<Producto>>.Exito(new List<Producto>());

            var productos = await _context.Producto
                .AsNoTracking()
                .Where(p => productoIds.Contains(p.Id) &&
                            p.Estado == (char)Domain.Enums.EstadoEnum.Activo)
                .ToListAsync();

            foreach(var producto in productos)
            {
                var productoMapeado = new Producto()
                {
                    Id = producto.Id,
                    Codigo = producto.Codigo,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Existencia = producto.Existencia,
                    Version = producto.Version
                };
                resultado.Valor.Add(productoMapeado);
            }
            return resultado;
        }
        catch (System.Exception ex)
        {
            return Resultado<List<Producto>>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<bool>> ActualizarExistencias(List<Producto> productos)
    {
        try
        {
            var productoIds = productos.Select(p => p.Id).ToList();

            var entidades = await _context.Producto
                .Where(p => productoIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            foreach(var producto in productos)
            {
                var entidad = entidades[producto.Id];

                entidad.Existencia = producto.Existencia;
                entidad.FechaActualizacion = DateTime.UtcNow;

                _context.Entry(entidad).Property(p => p.Version).OriginalValue = producto.Version;
            }
            return Resultado<bool>.Exito(true);
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
}