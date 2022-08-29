using IPS.UserManagement.Application.Features.Resources.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace IPS.UserManagement.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config => config.AsScoped(), typeof(ApplicationExtensions).Assembly);
        // resources
        services.AddSingleton<ResourceRequestConverter>();
        services.AddSingleton<ResourceConverter>();
        return services;
    }
}
