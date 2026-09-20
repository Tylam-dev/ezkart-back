using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Auth.Domain.Constantes;


namespace Auth.Infrastructure.Persistencia.Semillas;
internal class SemillaRoles : ISemilla
{
    private readonly AuthDBContext _context;
    private readonly ILogger<SemillaRoles> _logger;
    public SemillaRoles(AuthDBContext context, ILogger<SemillaRoles> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task Sembrar()
    {
        var deseados = Rol.Todos;
        var ids = deseados.Select(r => r.Id).ToList();
        var existentes = await _context.Rol.Where(r => ids.Contains(r.Id)).ToDictionaryAsync(r => r.Id);

        foreach (var deseado in deseados)
        {
            if (existentes.TryGetValue(deseado.Id, out var actual))
            {
                if (actual.Nombre != deseado.Nombre)
                    _context.Entry(actual).Property(r => r.Nombre).CurrentValue = deseado.Nombre; 
            }
            else
            {
                var rolNuevo = new Auth.Infrastructure.Persistencia.Entidades.Rol()
                {
                    Id = deseado.Id,
                    Nombre = deseado.Nombre
                };
                await _context.AddAsync<Auth.Infrastructure.Persistencia.Entidades.Rol>(rolNuevo);
            }
        }
        await _context.SaveChangesAsync();

        _logger.LogInformation("Roles Creados");
    }
}