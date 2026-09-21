using Compras.Application;
using Compras.Application.Utilidades;
using Compras.Domain.Dominio;
using Compras.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Compras.Infrastructure.Persistencia.Repositorio;

internal class RepositorioCarrito : IRepositorioCarrito
{
    private readonly ComprasDBContext _context;

    public RepositorioCarrito(ComprasDBContext context)
    {
        _context = context;
    }

    public async Task<Resultado<Carrito?>> ObtenerCarritoPorUsuario(Guid usuarioId)
    {
        try
        {
            var resultado = Resultado<Carrito?>.Exito(null);

            var carrito = await _context.Carrito
                .Include(c => c.Items.Where(i => i.Estado == (char)EstadoEnum.Activo))
                .Where(c => c.UsuarioId == usuarioId &&
                            c.Estado == (char)EstadoEnum.Activo)
                .FirstOrDefaultAsync();

            if(carrito is null) return resultado;

            var carritoMapeado = new Carrito()
            {
                Id = carrito.Id,
                UsuarioId = carrito.UsuarioId
            };

            foreach(var item in carrito.Items) carritoMapeado.Items.Add(new CarritoItem()
            {
                ProductoId = item.ProductoId,
                Codigo = item.CodigoProducto,
                Nombre = item.NombreProducto,
                Cantidad = item.Cantidad
            });
            resultado.Valor = carritoMapeado;

            return resultado;
        }
        catch (System.Exception ex)
        {
            return Resultado<Carrito?>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<bool>> GuardarCarrito(Carrito carrito)
    {
        try
        {
            var entidad = await _context.Carrito
                .Include(c => c.Items.Where(i => i.Estado == (char)EstadoEnum.Activo))
                .FirstOrDefaultAsync(c => c.Id == carrito.Id &&
                                          c.Estado == (char)EstadoEnum.Activo);

            if(entidad is null)
            {
                entidad = new Entidades.Carrito()
                {
                    Id = carrito.Id,
                    UsuarioId = carrito.UsuarioId
                };
                _context.Carrito.Add(entidad);
            }
            else entidad.FechaActualizacion = DateTime.UtcNow;

            foreach(var item in carrito.Items)
            {
                var itemActual = entidad.Items.FirstOrDefault(i => i.ProductoId == item.ProductoId);

                if(itemActual is null)
                {
                    _context.CarritoItem.Add(new Entidades.CarritoItem()
                    {
                        Id = Guid.NewGuid(),
                        CarritoId = entidad.Id,
                        ProductoId = item.ProductoId,
                        CodigoProducto = item.Codigo,
                        NombreProducto = item.Nombre,
                        Cantidad = item.Cantidad
                    });
                }
                else
                {
                    itemActual.NombreProducto = item.Nombre;
                    itemActual.Cantidad = item.Cantidad;
                    itemActual.FechaActualizacion = DateTime.UtcNow;
                }
            }
            await _context.SaveChangesAsync();

            return Resultado<bool>.Exito(true);
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<bool>> EliminarProductoCarrito(Guid carritoId, Guid productoId)
    {
        try
        {
            var item = await _context.CarritoItem
                .FirstOrDefaultAsync(i => i.CarritoId == carritoId &&
                                          i.ProductoId == productoId &&
                                          i.Estado == (char)EstadoEnum.Activo);

            if(item is not null)
            {
                item.Estado = (char)EstadoEnum.Eliminado;
                item.FechaEliminacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            return Resultado<bool>.Exito(true);
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<bool>> VaciarCarrito(Guid usuarioId)
    {
        try
        {
            var items = await _context.CarritoItem
                .Where(i => i.Carrito.UsuarioId == usuarioId &&
                            i.Estado == (char)EstadoEnum.Activo)
                .ToListAsync();

            foreach(var item in items)
            {
                item.Estado = (char)EstadoEnum.Eliminado;
                item.FechaEliminacion = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Resultado<bool>.Exito(true);
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
}
