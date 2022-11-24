using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Domain.Roles;

namespace IPS.UserManagement.Application.Features.Roles.Converters;

internal class RoleRequestConverter
{
    public CreateRequest ToDomain(CreateRoleCommandModel model)
    {
        return new CreateRequest(model.Name, model.Description);
    }
}
