using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Domain.Permissions;

namespace IPS.UserManagement.Application.Features.Permissions.Converters;

public class PermissionRequestConverter
{
    public CreateRequest ToDomain(CreatePermissionCommandModel model)
    {
        return new CreateRequest(model.Name, model.Description, model.Resource);
    }
}
