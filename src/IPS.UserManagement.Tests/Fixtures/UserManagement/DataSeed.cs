using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.DbContexts;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace IPS.UserManagement.Tests.Fixtures.UserManagement;

internal class DataSeed : BackgroundService
{
    private readonly ILookupNormalizer _normalizer;
    private readonly IPasswordHasher<ApplicationUserModel> _passwordHasher;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DataSeed(
        ILookupNormalizer normalizer,
        IPasswordHasher<ApplicationUserModel> passwordHasher,
        IServiceScopeFactory serviceScopeFactory)
    {
        _normalizer = normalizer;
        _passwordHasher = passwordHasher;
        _serviceScopeFactory = serviceScopeFactory;
    }

    private IReadOnlyCollection<ApplicationRoleModel> GetRoles()
    {
        return new List<ApplicationRoleModel>();
    }

    private IReadOnlyCollection<ApplicationUserModel> GetUsers()
    {
        var password = "secret";
        var bob = CreateApplicationUserModel("bob", "bob@contoso.com", password);
        var userManager = CreateApplicationUserModel("usermanager", "usermanager@contoso.com", password);
        return new[] { bob, userManager };
    }

    private ApplicationUserModel CreateApplicationUserModel(string userName, string email, string password)
    {
        var user = new ApplicationUserModel
        {
            UserName = userName,
            Email = email,
            NormalizedUserName = _normalizer.NormalizeName(userName),
            NormalizedEmail = _normalizer.NormalizeEmail(email),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("N")
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        return user;
    }

    private IReadOnlyCollection<ApplicationRoleClaim> GetRoleClaims()
    {
        return new List<ApplicationRoleClaim>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        await dbContext.Roles.AddRangeAsync(GetRoles(), stoppingToken);
        await dbContext.RoleClaims.AddRangeAsync(GetRoleClaims(), stoppingToken);
        await dbContext.Users.AddRangeAsync(GetUsers(), stoppingToken);
        await dbContext.SaveChangesAsync(stoppingToken);
    }
}
