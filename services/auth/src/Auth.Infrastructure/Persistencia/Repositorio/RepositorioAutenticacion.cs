using Auth.Application;
using Auth.Application.Utilidades;
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
                .Where(u => u.NombreUsuario == nombreUsuario)
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
    public async Task<Resultado<bool>> GuardarRefresToken(RefreshToken refreshToken)
    {
        try
        {
            var resultado = Resultado<bool>.Exito(true);
            
            var refreshTokenNuevo = new Auth.Infrastructure.Persistencia.Entidades.RefreshToken()
            {
                TokenHash = refreshToken.TokenHash,
                Expiracion = refreshToken.Expiracion
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
                .FirstAsync(t => t.TokenHash ==  refreshTokenAnterior.TokenHash);

            tokenAnterior.TokenHash = refreshTokenNuevo.TokenHash;
            tokenAnterior.Expiracion = refreshTokenNuevo.Expiracion;

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
}