using Inventario.Application;
using Inventario.Infrastructure.Persistencia.Repositorio;
using Inventario.Infrastructure.Persistencia.Semillas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventario.Infrastructure;

public static class InyeccionDependencia
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Falta ConnectionStrings:Default");

        services.AddDbContext<InventarioDBContext>(o => o.UseNpgsql(cs));
        AgregarServiciosSemilla(services);

        services.AddScoped<IRepositorioInventario, RepositorioInvenario>();
        services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();
        return services;
    }
    private static void AgregarServiciosSemilla(this IServiceCollection services)
    {
        services.AddScoped<SemillaProductos>();
        services.AddScoped<SemilleroGeneral>();
    }
    public static async Task InicializarBaseDatosAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<InventarioDBContext>();
        await db.Database.MigrateAsync();

        await scope.ServiceProvider
            .GetRequiredService<SemilleroGeneral>()
            .EjecutarSemillas();
    }
}
