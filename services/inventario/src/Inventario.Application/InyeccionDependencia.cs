using Invetario.Appliccation.IServcio;
using Invetario.Appliccation.Servcio;
using Microsoft.Extensions.DependencyInjection;

namespace Inventario.Application;

public static class InyeccionDependencia
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IInvetarioServicio, InvetarioServicio>();
        return services;
    }
}
