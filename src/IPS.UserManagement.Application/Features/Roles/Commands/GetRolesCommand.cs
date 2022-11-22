using IPS.UserManagement.Application.Features.Roles.Models;

namespace IPS.UserManagement.Application.Features.Roles.Commands;

public class GetRolesCommand : IRequest<List<RoleQueryModel>>
{
    private class GetRolesCommandHandler : IRequestHandler<GetRolesCommand, List<RoleQueryModel>>
    {
        public Task<List<RoleQueryModel>> Handle(GetRolesCommand request, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }
    }
}
