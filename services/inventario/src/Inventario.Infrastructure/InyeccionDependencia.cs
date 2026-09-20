using Inventario.Application;
using Inventario.Infrastructure.Persistencia.Repositorio;
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

        services.AddScoped<IRepositorioInventario, RepositorioInvenario>();
        services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();
        return services;
    }
}
