using IPS.UserManagement.Domain.Resources;
using IPS.UserManagement.Repositories.IdentityServer.Data.Migrations;
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
        var assembly = typeof(MigrationLocator).Assembly.GetName().Name ??
            throw new InvalidOperationException("Could not get assembly name");
        var identityServerEnabled = configuration.GetValue<bool>("IdentityServer:Enabled");
        var connectionString = configuration.GetConnectionString("SqlServer");
        services.AddIdentityServer()
            .AddConfigurationStore(
                store =>
                {
                    store.ConfigureDbContext = builder => builder.UseSqlite(
                        connectionString,
                        sql => sql.MigrationsAssembly(assembly)
                    );
                })
            .AddOperationalStore(
                store =>
                {
                    store.ConfigureDbContext = builder => builder.UseSqlite(
                        connectionString,
                        sql => sql.MigrationsAssembly(assembly)
                    );
                });
        if (identityServerEnabled)
        {
            services.AddHostedService<MigrationExecutor>();
        }
        // resources
        services.AddScoped<IResourceRepository, ResourceRepository>();
        return services;
    }
}
