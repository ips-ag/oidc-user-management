using IPS.UserManagement.Application.Features.Resources.Models;

namespace IPS.UserManagement.Application.Features.Resources.Commands;

public class CreateResourceCommand : IRequest<ResourceQueryModel>
{
    private class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand, ResourceQueryModel>
    {
        public Task<ResourceQueryModel> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
