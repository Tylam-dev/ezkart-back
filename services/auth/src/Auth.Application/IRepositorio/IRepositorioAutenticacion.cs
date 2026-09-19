namespace Auth.Application;

public interface IRepositorioAutenticacion
{
    Task<LoginDTO> LoginAsync(LoginDTO loginDTO);
    Task LogoutAsync(Guid id);
}
