using Duende.IdentityServer.EntityFramework.Storage;
using IPS.UserManagement.Domain.Resources;
using IPS.UserManagement.Repositories.IdentityServer.Resources;
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
        var connectionString = configuration.GetConnectionString("SqlServer") ??
            throw new InvalidOperationException("SQL Server ConnectionString not configured");
        services.AddConfigurationDbContext(
            store => store.ConfigureDbContext = builder => builder.UseSqlite(connectionString));
        // resources
        services.AddScoped<IResourceRepository, ResourceRepository>();
        return services;
    }
}
