using System.Net;
using System.Net.Http.Json;
using Compras.Application.DTOs;
using Compras.Application.Exepciones;
using Compras.Application.Utilidades;
using Compras.Domain.Exepcion;

namespace Compras.Api.Servicio;

public class ClienteHttpInventario : IClienteHttpInventario
{
    private readonly HttpClient _httpClient;

    public ClienteHttpInventario(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Resultado<ProductoInventarioDTO>> BuscarProducto(Guid productoId, string codigo)
    {
        try
        {
            var respuesta = await _httpClient.GetAsync(
                $"products/buscar?productoId={productoId}&codigo={Uri.EscapeDataString(codigo)}");

            if (respuesta.StatusCode == HttpStatusCode.NotFound)
                return Resultado<ProductoInventarioDTO>.Error("Articulo no encontrado", new ExepcionArticuloNoEncontrado());

            if (!respuesta.IsSuccessStatusCode)
                return Resultado<ProductoInventarioDTO>.Error("Inventario no disponible", new ExepcionInventarioNoDisponible());

            var producto = await respuesta.Content.ReadFromJsonAsync<ProductoInventarioDTO>();

            if (producto is null)
                return Resultado<ProductoInventarioDTO>.Error("Respuesta invalida de inventario", new ExepcionInventarioNoDisponible());

            return Resultado<ProductoInventarioDTO>.Exito(producto);
        }
        catch (System.Exception ex)
        {
            return Resultado<ProductoInventarioDTO>.Error("Inventario no disponible", new ExepcionInventarioNoDisponible(ex));
        }
    }

    public async Task<Resultado<bool>> DisminuirInventario(DisminuirInventarioDTO disminuirInventarioDTO)
    {
        try
        {
            var respuesta = await _httpClient.PostAsJsonAsync("products/disminuir", disminuirInventarioDTO);

            if (respuesta.IsSuccessStatusCode) return Resultado<bool>.Exito(true);

            var mensaje = await ObtenerMensajeError(respuesta);

            switch (respuesta.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Resultado<bool>.Error(mensaje, new ExepcionDominio(mensaje));
                case HttpStatusCode.NotFound:
                    return Resultado<bool>.Error(mensaje, new ExepcionArticuloNoEncontrado());
                case HttpStatusCode.Conflict:
                    return Resultado<bool>.Error(mensaje, new ExepcionStockInsuficiente(mensaje));
                default:
                    return Resultado<bool>.Error("Inventario no disponible", new ExepcionInventarioNoDisponible());
            }
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Inventario no disponible", new ExepcionInventarioNoDisponible(ex));
        }
    }

    private static async Task<string> ObtenerMensajeError(HttpResponseMessage respuesta)
    {
        try
        {
            var error = await respuesta.Content.ReadFromJsonAsync<ErrorInventario>();

            return error?.Message ?? "Error en inventario";
        }
        catch (System.Exception)
        {
            return "Error en inventario";
        }
    }

    private class ErrorInventario
    {
        public string? Message { get; set; }
    }
}
