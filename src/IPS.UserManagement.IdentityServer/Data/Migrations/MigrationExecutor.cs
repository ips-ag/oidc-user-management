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
        var persistedGrandContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
        await persistedGrandContext.Database.MigrateAsync(stoppingToken);
        var configurationContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        await configurationContext.Database.MigrateAsync(stoppingToken);
        // TODO: move DB seed to migration
        if (!configurationContext.Clients.Any())
        {
            foreach (var client in Config.GetClients())
            {
                configurationContext.Clients.Add(client.ToEntity());
            }
            await configurationContext.SaveChangesAsync(stoppingToken);
        }
        if (!configurationContext.IdentityResources.Any())
        {
            foreach (var resource in Config.GetIdentityResources())
            {
                configurationContext.IdentityResources.Add(resource.ToEntity());
            }
            await configurationContext.SaveChangesAsync(stoppingToken);
        }
        if (!configurationContext.ApiScopes.Any())
        {
            foreach (var resource in Config.GetScopes())
            {
                configurationContext.ApiScopes.Add(resource.ToEntity());
            }
            await configurationContext.SaveChangesAsync(stoppingToken);
        }
        if (!configurationContext.ApiResources.Any())
        {
            foreach (var resource in Config.GetApis())
            {
                configurationContext.ApiResources.Add(resource.ToEntity());
            }
            await configurationContext.SaveChangesAsync(stoppingToken);
        }
    }
}
