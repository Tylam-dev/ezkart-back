using Compras.Application;
using Compras.Application.Utilidades;
using Compras.Application.Utilidades.Queries;
using Compras.Domain.Dominio;
using Compras.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Compras.Infrastructure.Persistencia.Repositorio;

internal class RepositorioCompras : IRepositorioCompras
{
    private readonly ComprasDBContext _context;

    public RepositorioCompras(ComprasDBContext context)
    {
        _context = context;
    }

    public async Task<Resultado<Paginacion<Orden>>> ObtenerOrdenesPaginado(
        Guid usuarioId,
        PaginacionQuery<OrdenesQuery> queryOrdenes
    )
    {
        try
        {
            var query = _context.Orden
                .Where(o => o.UsuarioId == usuarioId &&
                            o.Estado == EstadoEnum.Activo);

            var filtro = queryOrdenes.Filtro;

            if(filtro is not null && filtro.EstadoOrden.HasValue)
                query = query.Where(o => o.EstadoOrden == filtro.EstadoOrden.Value);

            if(filtro is not null && filtro.FechaDesde.HasValue)
                query = query.Where(o => o.FechaCreacion >= filtro.FechaDesde.Value);

            if(filtro is not null && filtro.FechaHasta.HasValue)
                query = query.Where(o => o.FechaCreacion <= filtro.FechaHasta.Value);

            var totalOrdenes = await query.CountAsync();

            var ordenes = await query
                .Include(o => o.Detalles)
                .Include(o => o.DescuentoTemporada)
                .OrderByDescending(o => o.FechaCreacion)
                .Skip(queryOrdenes.Saltos)
                .Take(queryOrdenes.TamanoPagina)
                .ToListAsync();

            var totalPaginas = (int)Math.Ceiling(
                totalOrdenes / (double)queryOrdenes.TamanoPagina);

            var listaOrdenes = new List<Orden>();

            foreach(var orden in ordenes)
            {
                var ordenMapeada = new Orden()
                {
                    Id = orden.Id,
                    UsuarioId = orden.UsuarioId,
                    FechaCreacion = orden.FechaCreacion,
                    EstadoOrden = orden.EstadoOrden
                };

                if(orden.DescuentoTemporada is not null) ordenMapeada.DescuentoTemporada = new DescuentoTemporada()
                {
                    Id = orden.DescuentoTemporada.Id,
                    Nombre = orden.DescuentoTemporada.Nombre,
                    Porcentaje = orden.DescuentoTemporada.Porcentaje,
                    FechaDesde = orden.DescuentoTemporada.FechaDesde,
                    FechaHasta = orden.DescuentoTemporada.FechaHasta,
                    FechaCreacion = orden.DescuentoTemporada.FechaCreacion
                };

                foreach(var detalle in orden.Detalles) ordenMapeada.Detalles.Add(new OrdenDetalle()
                {
                    ProductoId = detalle.ProductoId,
                    Codigo = detalle.CodigoProducto,
                    Nombre = detalle.NombreProducto,
                    PrecioUnitario = detalle.PrecioUnitario,
                    Cantidad = detalle.Cantidad
                });

                listaOrdenes.Add(ordenMapeada);
            }

            var informacionPaginada = new Paginacion<Orden>()
            {
                Items = listaOrdenes,
                Pagina = queryOrdenes.Pagina,
                TamanoPagina = queryOrdenes.TamanoPagina,
                TotalItems = totalOrdenes,
                TotalPaginas = totalPaginas
            };
            return Resultado<Paginacion<Orden>>.Exito(informacionPaginada);
        }
        catch (System.Exception ex)
        {
            return Resultado<Paginacion<Orden>>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<Orden?>> ObtenerOrdenPorId(Guid usuarioId, Guid ordenId)
    {
        try
        {
            var resultado = Resultado<Orden?>.Exito(null);

            var orden = await _context.Orden
                .Include(o => o.Detalles)
                .Include(o => o.DescuentoTemporada)
                .Where(o => o.Id == ordenId &&
                            o.UsuarioId == usuarioId &&
                            o.Estado == EstadoEnum.Activo)
                .FirstOrDefaultAsync();

            if(orden is null) return resultado;

            var ordenMapeada = new Orden()
            {
                Id = orden.Id,
                UsuarioId = orden.UsuarioId,
                FechaCreacion = orden.FechaCreacion,
                EstadoOrden = orden.EstadoOrden
            };

            if(orden.DescuentoTemporada is not null) ordenMapeada.DescuentoTemporada = new DescuentoTemporada()
            {
                Id = orden.DescuentoTemporada.Id,
                Nombre = orden.DescuentoTemporada.Nombre,
                Porcentaje = orden.DescuentoTemporada.Porcentaje,
                FechaDesde = orden.DescuentoTemporada.FechaDesde,
                FechaHasta = orden.DescuentoTemporada.FechaHasta,
                FechaCreacion = orden.DescuentoTemporada.FechaCreacion
            };

            foreach(var detalle in orden.Detalles) ordenMapeada.Detalles.Add(new OrdenDetalle()
            {
                ProductoId = detalle.ProductoId,
                Codigo = detalle.CodigoProducto,
                Nombre = detalle.NombreProducto,
                PrecioUnitario = detalle.PrecioUnitario,
                Cantidad = detalle.Cantidad
            });
            resultado.Valor = ordenMapeada;

            return resultado;
        }
        catch (System.Exception ex)
        {
            return Resultado<Orden?>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<bool>> GuardarOrden(Orden orden)
    {
        try
        {
            _context.Orden.Add(new Entidades.Orden()
            {
                Id = orden.Id,
                UsuarioId = orden.UsuarioId,
                FechaCreacion = orden.FechaCreacion,
                EstadoOrden = orden.EstadoOrden,
                Total = orden.Total,
                DescuentoTemporadaId = orden.DescuentoTemporada?.Id
            });

            foreach(var detalle in orden.Detalles) _context.OrdenDetalle.Add(new Entidades.OrdenDetalle()
            {
                Id = Guid.NewGuid(),
                OrdenId = orden.Id,
                ProductoId = detalle.ProductoId,
                CodigoProducto = detalle.Codigo,
                NombreProducto = detalle.Nombre,
                PrecioUnitario = detalle.PrecioUnitario,
                Cantidad = detalle.Cantidad
            });

            await _context.SaveChangesAsync();

            return Resultado<bool>.Exito(true);
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<List<DescuentoTemporada>>> ObtenerDescuentosVigentes(DateTime fecha)
    {
        try
        {
            var resultado = Resultado<List<DescuentoTemporada>>.Exito(new List<DescuentoTemporada>());

            var descuentos = await _context.DescuentoTemporada
                .Where(d => d.Estado == EstadoEnum.Activo &&
                            d.FechaDesde <= fecha &&
                            d.FechaHasta >= fecha)
                .ToListAsync();

            foreach(var descuento in descuentos) resultado.Valor!.Add(new DescuentoTemporada()
            {
                Id = descuento.Id,
                Nombre = descuento.Nombre,
                Porcentaje = descuento.Porcentaje,
                FechaDesde = descuento.FechaDesde,
                FechaHasta = descuento.FechaHasta,
                FechaCreacion = descuento.FechaCreacion
            });

            return resultado;
        }
        catch (System.Exception ex)
        {
            return Resultado<List<DescuentoTemporada>>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<DescuentoTemporada?>> ObtenerDescuentoPorId(Guid descuentoId)
    {
        try
        {
            var resultado = Resultado<DescuentoTemporada?>.Exito(null);

            var descuento = await _context.DescuentoTemporada
                .Where(d => d.Id == descuentoId &&
                            d.Estado == EstadoEnum.Activo)
                .FirstOrDefaultAsync();

            if(descuento is null) return resultado;

            var descuentoMapeado = new DescuentoTemporada()
            {
                Id = descuento.Id,
                Nombre = descuento.Nombre,
                Porcentaje = descuento.Porcentaje,
                FechaDesde = descuento.FechaDesde,
                FechaHasta = descuento.FechaHasta,
                FechaCreacion = descuento.FechaCreacion
            };
            resultado.Valor = descuentoMapeado;

            return resultado;
        }
        catch (System.Exception ex)
        {
            return Resultado<DescuentoTemporada?>.Error("Error en DB", ex);
        }
    }
}
