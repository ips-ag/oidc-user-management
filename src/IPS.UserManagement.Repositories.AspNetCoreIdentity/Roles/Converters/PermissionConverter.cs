using IPS.UserManagement.Domain.Permissions;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.Roles.Converters;

internal class PermissionConverter
{
    public ApplicationRoleClaim ToModel(string roleId, Permission permission)
    {
        return new ApplicationRoleClaim { ClaimType = permission.Name, ClaimValue = permission.Name, RoleId = roleId };
    }
}
