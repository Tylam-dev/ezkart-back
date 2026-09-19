using Auth.Application;

public class AutenticacionServicio : IAutenticacionServicio
{
    private readonly IRepositorioAutenticacion _repositorioAutenticacion;
    public AutenticacionServicio(IRepositorioAutenticacion autenticacionRepositorio)
    {
        _repositorioAutenticacion = autenticacionRepositorio;
    }
    public Task<LoginDTO> LoginAsync(LoginDTO loginDTO)
    {
        throw new NotImplementedException();
    }
    public Task LogoutAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}