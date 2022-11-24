using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace IPS.UserManagement.IdentityServer.Data.Migrations;

internal class MigrationExecutor : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MigrationExecutor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var services = scope.ServiceProvider;
        var persistedGrantContext = services.GetRequiredService<PersistedGrantDbContext>();
        await persistedGrantContext.Database.MigrateAsync(stoppingToken);
        var configurationContext = services.GetRequiredService<ConfigurationDbContext>();
        await configurationContext.Database.MigrateAsync(stoppingToken);
        var seed = services.GetRequiredService<IDataSeed>();
        // TODO: move DB seed to migration
        if (!configurationContext.Clients.Any())
        {
            foreach (var client in seed.GetClients())
            {
                configurationContext.Clients.Add(client.ToEntity());
            }
            await configurationContext.SaveChangesAsync(stoppingToken);
        }
        if (!configurationContext.IdentityResources.Any())
        {
            foreach (var resource in seed.GetIdentityResources())
            {
                configurationContext.IdentityResources.Add(resource.ToEntity());
            }
            await configurationContext.SaveChangesAsync(stoppingToken);
        }
        if (!configurationContext.ApiScopes.Any())
        {
            foreach (var resource in seed.GetScopes())
            {
                configurationContext.ApiScopes.Add(resource.ToEntity());
            }
            await configurationContext.SaveChangesAsync(stoppingToken);
        }
        if (!configurationContext.ApiResources.Any())
        {
            foreach (var resource in seed.GetApis())
            {
                configurationContext.ApiResources.Add(resource.ToEntity());
            }
            await configurationContext.SaveChangesAsync(stoppingToken);
        }
    }
}
