using IPS.UserManagement.Domain.Permissions;
using IPS.UserManagement.Repositories.EntityFramework.Models;

namespace IPS.UserManagement.Repositories.Roles.Converters;

internal class RolePermissionConverter
{
    public RolePermissionModel ToModel(Permission permission)
    {
        return new RolePermissionModel { PermissionId = permission.Id };
    }
}
