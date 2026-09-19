namespace Auth.Application.IServicios;
public interface IAutenticacionServicio
{
    Task<LoggedDTO> LoginAsync(LoginDTO loginDTO);

    Task<bool> LogoutAsync(string hashRefreshToken);

    Task<TokenAcceso> RefrescarToken(string refreshToken, Guid usuarioId);
}