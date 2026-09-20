namespace Auth.Infrastructure.Persistencia.Semillas;

internal sealed class SemilleroGeneral
{
    private readonly SemillaRoles _roles;
    private readonly SemillaUsuarios _usuarios;
    public SemilleroGeneral(
        SemillaRoles roles, 
        SemillaUsuarios usuarios
    )
    {
        _roles = roles;
        _usuarios = usuarios;
    }
    public async Task EjecutarSemillas()
    {
        await _roles.Sembrar();
        await _usuarios.Sembrar();
    }
}