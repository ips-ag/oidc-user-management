using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;
using Microsoft.AspNetCore.Identity;

namespace IPS.UserManagement.Tests.Fixtures.UserManagement;

internal class DataSeed : IDataSeed
{
    private readonly ILookupNormalizer _normalizer;
    private readonly IPasswordHasher<ApplicationUserModel> _passwordHasher;

    public DataSeed(ILookupNormalizer normalizer, IPasswordHasher<ApplicationUserModel> passwordHasher)
    {
        _normalizer = normalizer;
        _passwordHasher = passwordHasher;
    }

    public IReadOnlyCollection<ApplicationRoleModel> GetRoles()
    {
        return new List<ApplicationRoleModel>();
    }

    public IReadOnlyCollection<ApplicationUserModel> GetUsers()
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

    public IReadOnlyCollection<ApplicationRoleClaim> GetRoleClaims()
    {
        return new List<ApplicationRoleClaim>();
    }
}
