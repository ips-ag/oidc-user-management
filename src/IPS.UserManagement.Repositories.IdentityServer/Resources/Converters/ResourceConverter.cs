using Duende.IdentityServer.EntityFramework.Entities;
using IPS.UserManagement.Domain.Resources;

namespace IPS.UserManagement.Repositories.IdentityServer.Resources.Converters;

internal class ResourceConverter
{
    public Resource ToDomain(ApiResource model)
    {
        return new Resource(
            model.Id.ToString("D"),
            model.Name,
            model.Description,
            model.DisplayName);
    }
}
