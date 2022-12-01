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
        var seed = services.GetRequiredService<IDataSeed>();
        if (!await dbContext.Roles.AnyAsync(stoppingToken))
        {
            await dbContext.Roles.AddRangeAsync(seed.GetRoles(), stoppingToken);
            await dbContext.SaveChangesAsync(stoppingToken);
        }
        if (!await dbContext.RoleClaims.AnyAsync(stoppingToken))
        {
            await dbContext.RoleClaims.AddRangeAsync(seed.GetRoleClaims(), stoppingToken);
            await dbContext.SaveChangesAsync(stoppingToken);
        }
        if (!await dbContext.Users.AnyAsync(stoppingToken))
        {
            await dbContext.Users.AddRangeAsync(seed.GetUsers(), stoppingToken);
            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}
