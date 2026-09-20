using System.Net;
using System.Net.Http.Json;
using Compras.Application.DTOs;
using Compras.Application.Exepciones;
using Compras.Application.Utilidades;

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
}
