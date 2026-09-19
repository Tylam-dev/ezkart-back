using Auth.Application;
using Auth.Application.IServicios;
using Auth.Infrastructure.Persistencia.Repositorio;
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
        services.AddSingleton<IContraseniaHasher, ContraseniaHasher>();
        
        services.AddScoped<IRepositorioAutenticacion, RepositorioAutenticacion>();
        return services;
    }
}