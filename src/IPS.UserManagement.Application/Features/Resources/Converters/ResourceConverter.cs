using IPS.UserManagement.Application.Features.Resources.Models;
using IPS.UserManagement.Domain.Resources;

namespace IPS.UserManagement.Application.Features.Resources.Converters;

internal class ResourceConverter
{
    public List<ResourceQueryModel> ToModel(IReadOnlyCollection<Resource> resources)
    {
        return resources.Select(ToModel).ToList();
    }

    public ResourceQueryModel ToModel(Resource resource)
    {
        return new ResourceQueryModel
        {
            Id = resource.Id, Name = resource.Name, Description = resource.Description, Location = resource.Location
        };
    }
}
