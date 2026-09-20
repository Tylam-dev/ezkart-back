using Auth.Application.IServicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Persistencia.Semillas;
internal class SemillaUsuarios : ISemilla
{
    private readonly AuthDBContext _context;
    private readonly ILogger<SemillaUsuarios> _logger;
    private readonly IContraseniaHasher _hasher;
    private readonly IConfiguration _config;
    public SemillaUsuarios(
    AuthDBContext context,
    ILogger<SemillaUsuarios> logger,
    IContraseniaHasher hasher,
    IConfiguration config)
{
    _context = context;
    _logger = logger;
    _hasher = hasher;
    _config = config;
}
    public async Task Sembrar()
    {
        var perfiles = new[]
        {
            (Prefijo: "Admin",   Nombre: "Administrador",     RolId: Auth.Domain.Constantes.Rol.Administrador.Id),
            (Prefijo: "Cliente", Nombre: "Cliente de prueba", RolId: Auth.Domain.Constantes.Rol.Cliente.Id)
        };

        foreach (var (prefijo, nombre, rolId) in perfiles)
        {
            var nombreUsuario = _config[$"Seed:{prefijo}:Usuario"];
            var contrasenia   = _config[$"Seed:{prefijo}:Contrasenia"];

            if (string.IsNullOrWhiteSpace(nombreUsuario)
                || string.IsNullOrWhiteSpace(contrasenia))
            {
                _logger.LogInformation(
                    "Semilla de usuario {Perfil} omitida: falta configuración Seed", prefijo);
                continue;
            }

            var existente = await _context.Usuario
                .SingleOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

            if (existente is null)
            {
                _context.Usuario.Add(new Entidades.Usuario
                {
                    Id = Guid.NewGuid(),
                    Nombre = nombre,
                    CorreoElectronico = "correo@cualquiera.com",
                    NombreUsuario = nombreUsuario,
                    Contrasenia = _hasher.GenerarHash(contrasenia), 
                    RolId = rolId
                });
                _logger.LogInformation("Usuario {Perfil} creado", prefijo);
            }
            else if (existente.RolId != rolId)
            {
                existente.RolId = rolId; 
                _logger.LogInformation("Rol del usuario {Perfil} actualizado", prefijo);
            }
        }

        await _context.SaveChangesAsync();
    }
}