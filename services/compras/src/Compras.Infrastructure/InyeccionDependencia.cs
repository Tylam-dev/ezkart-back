using Compras.Application;
using Compras.Infrastructure.Persistencia.Repositorio;
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

        services.AddScoped<IRepositorioCarrito, RepositorioCarrito>();
        services.AddScoped<IRepositorioCompras, RepositorioCompras>();
        return services;
    }
}
