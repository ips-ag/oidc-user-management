using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Domain.Permissions;

namespace IPS.UserManagement.Application.Features.Permissions.Converters;

public class PermissionConverter
{
    public PermissionQueryModel ToModel(Permission permission)
    {
        return new PermissionQueryModel
        {
            Id = permission.Id, Name = permission.Name, Description = permission.Description
        };
    }

    public List<PermissionQueryModel> ToModel(IReadOnlyCollection<Permission> permissions)
    {
        return permissions.Select(ToModel).ToList();
    }
}
