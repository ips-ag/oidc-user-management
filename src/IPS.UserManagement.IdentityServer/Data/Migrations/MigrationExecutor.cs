using Duende.IdentityServer.EntityFramework.DbContexts;
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
    }
}
