using IPS.UserManagement.Application.Features.Resources.Models;

namespace IPS.UserManagement.Application.Features.Resources.Commands;

public class GetResourcesCommand : IRequest<List<ResourceQueryModel>>
{
    private class GetResourcesCommandHandler : IRequestHandler<GetResourcesCommand, List<ResourceQueryModel>>
    {
        public Task<List<ResourceQueryModel>> Handle(GetResourcesCommand request, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }
    }
}
