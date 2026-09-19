using Auth.Application;

namespace Auth.Infrastructure.Persistencia.Repositorio;

public class RepositorioAutenticacion : IRepositorioAutenticacion
{
    private readonly AuthDBContext _context;

    public RepositorioAutenticacion(AuthDBContext context)
    {
        _context = context;
    }

    public async Task<LoginDTO> LoginAsync(LoginDTO loginDTO)
    {
        throw new NotImplementedException();
    }

    public async Task LogoutAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}