using Compras.Application;
using Compras.Infrastructure.Persistencia.Repositorio;
using Compras.Infrastructure.Persistencia.Semillas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Compras.Infrastructure;

public static class InyeccionDependencia
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Falta ConnectionStrings:Default");

        services.AddDbContext<ComprasDBContext>(o => o.UseNpgsql(cs));
        AgregarServiciosSemilla(services);

        services.AddScoped<IRepositorioCarrito, RepositorioCarrito>();
        services.AddScoped<IRepositorioCompras, RepositorioCompras>();
        return services;
    }
    private static void AgregarServiciosSemilla(this IServiceCollection services)
    {
        services.AddScoped<SemillaDescuentos>();
        services.AddScoped<SemilleroGeneral>();
    }
    public static async Task InicializarBaseDatosAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ComprasDBContext>();
        await db.Database.MigrateAsync();

        await scope.ServiceProvider
            .GetRequiredService<SemilleroGeneral>()
            .EjecutarSemillas();
    }
}
