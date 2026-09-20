using Inventario.Application;
using Inventario.Application.Exepciones;
using Inventario.Application.Utilidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventario.Infrastructure.Persistencia.Repositorio;

internal class UnidadTrabajo : IUnidadTrabajo
{
    private readonly InventarioDBContext _context;
    private IDbContextTransaction? _transaccion;

    public UnidadTrabajo(InventarioDBContext context)
    {
        _context = context;
    }

    public async Task<Resultado<bool>> IniciarTransaccion()
    {
        try
        {
            _transaccion = await _context.Database.BeginTransactionAsync();

            return Resultado<bool>.Exito(true);
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<bool>> Confirmar()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _transaccion!.CommitAsync();

            return Resultado<bool>.Exito(true);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Resultado<bool>.Error("Conflicto de concurrencia", new ExepcionConcurrencia());
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<bool>> Revertir()
    {
        try
        {
            if (_transaccion != null)
            {
                await _transaccion.RollbackAsync();
                await _transaccion.DisposeAsync();
                _transaccion = null;
            }
            _context.ChangeTracker.Clear();

            return Resultado<bool>.Exito(true);
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
}
