using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Domain.Roles;

namespace IPS.UserManagement.Application.Features.Roles.Converters;

internal class RoleConverter
{
    public RoleQueryModel ToModel(Role role)
    {
        return new RoleQueryModel { Id = role.Id, Name = role.Name, Description = role.Description };
    }

    public List<RoleQueryModel> ToModel(IReadOnlyCollection<Role> roles)
    {
        return roles.Select(ToModel).ToList();
    }
}
