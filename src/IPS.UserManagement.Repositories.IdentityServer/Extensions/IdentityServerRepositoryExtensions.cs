using Duende.IdentityServer.EntityFramework.Storage;
using IPS.UserManagement.Domain.Permissions;
using IPS.UserManagement.Domain.Resources;
using IPS.UserManagement.Repositories.IdentityServer.Permissions;
using IPS.UserManagement.Repositories.IdentityServer.Permissions.Converters;
using IPS.UserManagement.Repositories.IdentityServer.Resources;
using IPS.UserManagement.Repositories.IdentityServer.Resources.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IPS.UserManagement.Repositories.IdentityServer.Extensions;

public static class IdentityServerRepositoryExtensions
{
    public static IServiceCollection AddIdentityServerRepositories(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // identity server
        var connectionString = configuration.GetConnectionString("IdentityServer") ??
            throw new InvalidOperationException("IdentityServer ConnectionString not configured");
        services.AddConfigurationDbContext(
            store => store.ConfigureDbContext = builder => builder.UseSqlite(connectionString));
        // resources
        services.AddScoped<IResourceRepository, ResourceRepository>();
        services.AddSingleton<ResourceConverter>();
        // permissions
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddSingleton<PermissionConverter>();
        return services;
    }
}
