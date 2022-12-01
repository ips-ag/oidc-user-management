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
        var userName = "bob";
        var email = "bob@contoso.com";
        var bob = new ApplicationUserModel
        {
            UserName = userName,
            Email = email,
            NormalizedUserName = _normalizer.NormalizeName(userName),
            NormalizedEmail = _normalizer.NormalizeEmail(email),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("N")
        };
        bob.PasswordHash = _passwordHasher.HashPassword(bob, "secret");
        return new[] { bob };
    }

    public IReadOnlyCollection<ApplicationRoleClaim> GetRoleClaims()
    {
        return new List<ApplicationRoleClaim>();
    }
}
