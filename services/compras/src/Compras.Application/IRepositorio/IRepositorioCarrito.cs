using Compras.Application.Utilidades;
using Compras.Domain.Dominio;

namespace Compras.Application;

public interface IRepositorioCarrito
{
    Task<Resultado<Carrito?>> ObtenerCarritoPorUsuario(Guid usuarioId);
    Task<Resultado<bool>> GuardarCarrito(Carrito carrito);
}
