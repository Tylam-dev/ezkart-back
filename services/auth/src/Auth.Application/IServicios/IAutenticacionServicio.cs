using Auth.Application.Utilidades;
using Auth.Domain;

namespace Auth.Application.IServicios;
public interface IAutenticacionServicio
{
    Task<LoggedDTO> LoginAsync(LoginDTO loginDTO);

    Task<bool> LogoutAsync(string hashRefreshToken);
}