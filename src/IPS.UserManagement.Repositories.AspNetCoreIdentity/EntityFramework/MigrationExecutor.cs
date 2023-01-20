using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework;

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
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync(stoppingToken);
    }
}
