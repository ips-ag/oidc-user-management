using IPS.UserManagement.IdentityServer.Data;
using IPS.UserManagement.IdentityServer.Data.Migrations;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.Extensions;
using IPS.UserManagement.Repositories.IdentityServer.Extensions;
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
        services
            .AddAspNetCoreIdentityRepositories(configuration)
            .AddIdentityServerRepositories(configuration, false);
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
                })
            .AddAspNetIdentity<ApplicationUserModel>();
        services.AddSingleton<IDataSeed, DataSeed>();
        services.AddHostedService<MigrationExecutor>();
        return services;
    }
}
