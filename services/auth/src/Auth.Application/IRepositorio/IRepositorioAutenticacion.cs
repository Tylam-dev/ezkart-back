using Auth.Application.Utilidades;
using Auth.Domain;

namespace Auth.Application;

public interface IRepositorioAutenticacion
{
    Task<Resultado<Usuario?>> ObtenerUsuarioLogin(string nombreUsuario);
    Task<Resultado<Usuario?>> ObtenerUsuarioId(Guid idUsuario);
    Task<Resultado<bool>> GuardarRefresToken(RefreshToken refreshToken, Guid usuarioId);
    Task<Resultado<RefreshToken>> ActualizarToken(RefreshToken refreshTokenAnterior, RefreshToken refreshTokenNuevo);
    Task<Resultado<bool>> LogoutAsync(string refreshTokenHash);
    Task<Resultado<List<string>>>ObtenerHashRefreshTokensCaducados(DateTime fechaLimite);
    Task<Resultado<bool>>EliminarRefreshTokens(List<string>refreshTokenHashes);
    Task<Resultado<DateTime?>>ObtenerFechaCaducidadRefreshToken(string refreshTokenHash);
    Task<Resultado<Guid?>>ObtenerUsuarioIdRefreshToken(string refreshTokenHash);
}
