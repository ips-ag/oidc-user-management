using IPS.UserManagement.Domain.Roles;
using IPS.UserManagement.Repositories.EntityFramework.Models;

namespace IPS.UserManagement.Repositories.Roles.Converters;

internal class RoleConverter
{
    public Role ToDomain(RoleModel model)
    {
        return new Role(model.Id.ToString("D"), model.Name, model.Description);
    }
}
