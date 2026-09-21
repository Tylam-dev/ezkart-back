using Auth.Application;
using Auth.Application.Utilidades;
using Auth.Domain.Constantes;
using Auth.Domain;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistencia.Repositorio;

public class RepositorioAutenticacion : IRepositorioAutenticacion
{
    private readonly AuthDBContext _context;

    public RepositorioAutenticacion(AuthDBContext context)
    {
        _context = context;
    }

    public async Task<Resultado<Usuario?>> ObtenerUsuarioLogin(string nombreUsuario)
    {
        try
        {
            var resultado = Resultado<Usuario?>.Exito(null);
            var usuario = await _context.Usuario
                .Where(u => u.NombreUsuario == nombreUsuario &&
                            u.Estado == (char)EstadoEnum.Activo)
                .FirstOrDefaultAsync();

            if(usuario is null) return resultado;
            
            resultado.Valor =  new Usuario(
                usuario.Id,
                Rol.ObtenerPorId(usuario.RolId),
                usuario.Nombre,
                new Domain.ObjValor.CorreoElectronico(usuario.CorreoElectronico),
                usuario.CorreoElectronico,
                usuario.Contrasenia
            );
            return resultado;
        }
        catch (Exception ex)
        {
            return Resultado<Usuario?>.Error("Error en BD", ex);
        }
    }
    public async Task<Resultado<bool>> GuardarRefresToken(RefreshToken refreshToken, Guid usuarioId)
    {
        try
        {
            var resultado = Resultado<bool>.Exito(true);
            
            var refreshTokenNuevo = new Auth.Infrastructure.Persistencia.Entidades.RefreshToken()
            {
                TokenHash = refreshToken.TokenHash,
                Expiracion = refreshToken.Expiracion,
                UsuarioId = usuarioId
            };
            _context.Add<Auth.Infrastructure.Persistencia.Entidades.RefreshToken>(refreshTokenNuevo);
            await _context.SaveChangesAsync();

            return resultado;
        }
        catch (Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<RefreshToken>> ActualizarToken(RefreshToken refreshTokenAnterior, RefreshToken refreshTokenNuevo)
    {
        try
        {
            var tokenAnterior = await _context.RefreshToken
                .FirstOrDefaultAsync(t => t.TokenHash ==  refreshTokenAnterior.TokenHash);

            if(tokenAnterior is null)
                return Resultado<RefreshToken>.Error("Refresh token no encontrado", new CredencialesInvalidas());

            tokenAnterior.TokenHash = refreshTokenNuevo.TokenHash;
            tokenAnterior.Expiracion = refreshTokenNuevo.Expiracion;
            tokenAnterior.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Resultado<RefreshToken>.Exito(refreshTokenNuevo);
        }
        catch (Exception ex)
        {
            return Resultado<RefreshToken>.Error("Error en DB", ex);
        }
    }

    public async Task<Resultado<bool>> LogoutAsync(string refreshTokenHash)
    {
        try
        {
            var refreshToken = await _context.RefreshToken
                .FirstOrDefaultAsync(t => t.TokenHash == refreshTokenHash);

            if (refreshToken != null)
            {
                _context.RefreshToken.Remove(refreshToken);
                await _context.SaveChangesAsync();
            }
            return Resultado<bool>.Exito(true);
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<List<string>>>ObtenerHashRefreshTokensCaducados(DateTime fechaLimite)
    {
        try
        {
            var hashes = await _context.RefreshToken
                .Where(t => t.Expiracion <= fechaLimite)
                .Select(t => t.TokenHash)
                .ToListAsync();

            return Resultado<List<string>>.Exito(hashes);
        }
        catch (System.Exception ex)
        {
            return Resultado<List<string>>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<bool>>EliminarRefreshTokens(List<string>refreshTokenHashes)
    {
        try
        {
            var tokens = await _context.RefreshToken
                .Where(t => refreshTokenHashes.Contains(t.TokenHash))
                .ToListAsync();
            
            _context.RefreshToken.RemoveRange(tokens);

            await _context.SaveChangesAsync();

            return Resultado<bool>.Exito(true);
        }
        catch (System.Exception ex)
        {
            return Resultado<bool>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<DateTime?>>ObtenerFechaCaducidadRefreshToken(string refreshTokenHash)
    {
        try
        {
            var fecha = await _context.RefreshToken
                .Where(t => t.TokenHash == refreshTokenHash)
                .Select(t => (DateTime?)t.Expiracion)
                .FirstOrDefaultAsync();

            return Resultado<DateTime?>.Exito(fecha);
        }
        catch (System.Exception ex)
        {
            return Resultado<DateTime?>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<Guid?>>ObtenerUsuarioIdRefreshToken(string refreshTokenHash)
    {
        try
        {
            var usuarioId = await _context.RefreshToken
                .Where(t => t.TokenHash == refreshTokenHash)
                .Select(t => (Guid?)t.UsuarioId)
                .FirstOrDefaultAsync();

            return Resultado<Guid?>.Exito(usuarioId);
        }
        catch (System.Exception ex)
        {
            return Resultado<Guid?>.Error("Error en DB", ex);
        }
    }
    public async Task<Resultado<Usuario?>> ObtenerUsuarioId(Guid usuarioId)
    {
        try
        {
            var resultado = Resultado<Usuario?>.Exito(null);
            var usuario = await _context.Usuario
                .Where(u => u.Id == usuarioId &&
                            u.Estado == (char)EstadoEnum.Activo)
                .FirstOrDefaultAsync();
            
            if(usuario is null) return resultado;
            
            resultado.Valor =  new Usuario(
                usuario.Id,
                Rol.ObtenerPorId(usuario.RolId),
                usuario.Nombre,
                new Domain.ObjValor.CorreoElectronico(usuario.CorreoElectronico),
                usuario.CorreoElectronico,
                usuario.Contrasenia
            );
            return resultado;
        }
        catch (System.Exception ex)
        {
            return Resultado<Usuario?>.Error("Error en BD", ex);
        }
    }
    
}