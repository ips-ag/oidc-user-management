using IPS.UserManagement.Application.Features.Resources.Models;
using IPS.UserManagement.Domain.Resources;

namespace IPS.UserManagement.Application.Features.Resources.Converters;

internal class ResourceRequestConverter
{
    public CreateRequest ToDomain(CreateResourceCommandModel model)
    {
        return new CreateRequest(model.Name, model.Description, model.Location);
    }
}
