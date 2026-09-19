using Auth.Application.Utilidades;
using Auth.Domain;

namespace Auth.Application.IServicios;
public interface IJwtTokenServicio
{
    Resultado<TokenAcceso> GenerarAccessToken(Usuario usuario);
    Resultado<RefreshToken> GenerarRefreshToken();
}
    