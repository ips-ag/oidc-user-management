using Duende.IdentityServer.EntityFramework.Entities;
using IPS.UserManagement.Domain.Permissions;

namespace IPS.UserManagement.Repositories.IdentityServer.Permissions.Converters;

internal class PermissionConverter
{
    public Permission ToDomain(ApiScope model)
    {
        return new Permission(model.Id.ToString("D"), model.Name, model.Description);
    }
}
