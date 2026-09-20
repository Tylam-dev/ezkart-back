using Inventario.Application;
using Inventario.Application.DTOs;
using Inventario.Application.Exepciones;
using Inventario.Application.Utilidades;
using Inventario.Application.Utilidades.Queries;
using Inventario.Domain.Dominio;
using Inventario.Domain.Exepcion;
using Invetario.Appliccation.IServcio;

namespace Invetario.Appliccation.Servcio;

public class InvetarioServicio : IInvetarioServicio
{
    private const int MaximoIntentos = 3;
    private readonly IRepositorioInventario _repositorioInventario;
    private readonly IUnidadTrabajo _unidadTrabajo;
    public InvetarioServicio(
        IRepositorioInventario repositorioInventario,
        IUnidadTrabajo unidadTrabajo
    )
    {
        _repositorioInventario = repositorioInventario;
        _unidadTrabajo = unidadTrabajo;
    }
    public async Task<Paginacion<Producto>> ObtenerProductosFiltradosPaginado(
        PaginacionQuery<ProductosQuery> query
    )
    {
        var informacionPaginada = await _repositorioInventario.ObtenerProductosFiltradosPaginado(query);

        if(!informacionPaginada.Exitoso) throw informacionPaginada.Excepcion!;

        return informacionPaginada.Valor!;
    }
    public async Task<bool> DisminuirInventario(DisminuirInventarioDTO disminuirInventarioDTO)
    {
        if(disminuirInventarioDTO.Productos.Count == 0)
            throw new ExepcionDominio("Debe indicar al menos un producto.");

        var cantidades = disminuirInventarioDTO.Productos
            .GroupBy(p => p.ProductoId)
            .ToDictionary(g => g.Key, g => g.Sum(p => p.Cantidad));

        for(var intento = 1; intento <= MaximoIntentos; intento++)
        {
            var iniciada = await _unidadTrabajo.IniciarTransaccion();

            if(!iniciada.Exitoso) throw iniciada.Excepcion!;

            try
            {
                var productos = await _repositorioInventario.ObtenerListaProductoPorId(cantidades.Keys.ToList());

                if(!productos.Exitoso) throw productos.Excepcion!;

                if(productos.Valor!.Count != cantidades.Count) throw new ExepcionProductoNoEncontrado();

                foreach(var producto in productos.Valor) producto.Descontar(cantidades[producto.Id]);

                var actualizados = await _repositorioInventario.ActualizarExistencias(productos.Valor);

                if(!actualizados.Exitoso) throw actualizados.Excepcion!;

                var confirmado = await _unidadTrabajo.Confirmar();

                if(confirmado.Exitoso) return true;

                throw confirmado.Excepcion!;
            }
            catch (ExepcionConcurrencia) when (intento < MaximoIntentos)
            {
                await _unidadTrabajo.Revertir();
            }
            catch (Exception)
            {
                await _unidadTrabajo.Revertir();
                throw;
            }
        }
        throw new ExepcionConcurrencia();
    }
    public async Task<Producto?> ObtenerProductoPorId(Guid productoId)
    {
        var producto = await _repositorioInventario.ObtenerProductoPorId(productoId);

        if(!producto.Exitoso) throw producto.Excepcion!;

        return producto.Valor!;
    }
}