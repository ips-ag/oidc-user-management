using IPS.UserManagement.Repositories.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IPS.UserManagement.Repositories.EntityFramework;

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
        var dbContext = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();
        await dbContext.Database.MigrateAsync(stoppingToken);
    }
}
