using Auth.Application.Utilidades;
using Auth.Domain;

namespace Auth.Application;

public interface IRepositorioAutenticacion
{
    Task<Resultado<Usuario?>> ObtenerUsuarioLogin(string nombreUsuario);
    Task<Resultado<bool>> GuardarRefresToken(RefreshToken refreshToken);
    Task<Resultado<RefreshToken>> ActualizarToken(RefreshToken refreshTokenAnterior, RefreshToken refreshTokenNuevo);
    Task<Resultado<bool>> LogoutAsync(string refreshTokenHash);
    Task<Resultado<List<string>>>ObtenerHashRefreshTokensCaducados(DateTime fechaLimite);
    Task<Resultado<bool>>EliminarRefreshTokens(List<string>refreshTokenHashes);
}
