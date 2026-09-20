using Inventario.Application.Utilidades;

namespace Inventario.Application;

public interface IUnidadTrabajo
{
    Task<Resultado<bool>> IniciarTransaccion();
    Task<Resultado<bool>> Confirmar();
    Task<Resultado<bool>> Revertir();
}
