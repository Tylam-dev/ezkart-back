using Auth.Application;
using Auth.Application.IServicios;
using Auth.Infrastructure.Persistencia.Repositorio;
using Auth.Infrastructure.Persistencia.Semillas;
using Auth.Infrastructure.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;

public static class InyeccionDependencia
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Falta ConnectionStrings:Default");

        services.AddDbContext<AuthDBContext>(o => o.UseNpgsql(cs));
        AgregarServiciosSemilla(services);
        services.AddSingleton<IContraseniaHasher, ContraseniaHasher>();
        
        services.AddScoped<IRepositorioAutenticacion, RepositorioAutenticacion>();
        return services;
    }
    private static void AgregarServiciosSemilla(this IServiceCollection services)
    {
        services.AddScoped<SemillaRoles>();
        services.AddScoped<SemillaUsuarios>();
        services.AddScoped<SemilleroGeneral>();
    }
    public static async Task InicializarBaseDatosAsync(this IServiceProvider services)
{
    using var scope = services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<AuthDBContext>();
    await db.Database.MigrateAsync();

    await scope.ServiceProvider
        .GetRequiredService<SemilleroGeneral>()
        .EjecutarSemillas();
}
}