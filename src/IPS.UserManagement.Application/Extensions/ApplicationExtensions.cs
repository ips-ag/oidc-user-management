using IPS.UserManagement.Application.Features.Permissions.Converters;
using IPS.UserManagement.Application.Features.Resources.Converters;
using IPS.UserManagement.Application.Features.Roles.Converters;
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
        // permissions
        services.AddSingleton<PermissionRequestConverter>();
        services.AddSingleton<PermissionConverter>();
        // roles
        services.AddSingleton<RoleRequestConverter>();
        services.AddSingleton<RoleConverter>();
        return services;
    }
}
