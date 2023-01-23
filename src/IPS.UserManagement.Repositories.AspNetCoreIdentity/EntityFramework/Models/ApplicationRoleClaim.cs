using Microsoft.AspNetCore.Identity;

#pragma warning disable CS8765

#pragma warning disable CS8618

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public override string ClaimType { get; set; }
}
