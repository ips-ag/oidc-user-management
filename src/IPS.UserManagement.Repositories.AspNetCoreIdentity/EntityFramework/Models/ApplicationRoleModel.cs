using Microsoft.AspNetCore.Identity;

#pragma warning disable CS8765
#pragma warning disable CS8618

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;

public class ApplicationRoleModel : IdentityRole<string>
{
    public override string Name { get; set; }
    public override string NormalizedName { get; set; }
    public string? Description { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Updated { get; set; }
    public DateTime? LastAccessed { get; set; }
}
