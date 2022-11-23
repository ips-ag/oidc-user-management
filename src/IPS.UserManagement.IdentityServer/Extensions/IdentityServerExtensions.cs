using IPS.UserManagement.IdentityServer.Data.Migrations;
using Microsoft.EntityFrameworkCore;

namespace IPS.UserManagement.IdentityServer.Extensions;

public static class IdentityServerExtensions
{
    public static IServiceCollection ConfigureIdentityServer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var assembly = typeof(MigrationLocator).Assembly.GetName().Name ??
            throw new InvalidOperationException("Unable to locate assembly containing database migrations");
        var connectionString = configuration.GetConnectionString("IdentityServer")??
            throw new InvalidOperationException("IdentityServer ConnectionString not configured");
        services.AddIdentityServer()
            .AddConfigurationStore(
                store =>
                {
                    store.ConfigureDbContext = builder => builder.UseSqlite(
                        connectionString,
                        sql => sql.MigrationsAssembly(assembly));
                })
            .AddOperationalStore(
                store =>
                {
                    store.ConfigureDbContext = builder => builder.UseSqlite(
                        connectionString,
                        sql => sql.MigrationsAssembly(assembly));
                });
        services.AddHostedService<MigrationExecutor>();
        return services;
    }
}
