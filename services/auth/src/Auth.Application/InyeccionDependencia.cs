using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class InyeccionDependencia
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(InyeccionDependencia).Assembly);
        services.AddScoped<IAutenticacionServicio, AutenticacionServicio>();
        return services;
    }
}