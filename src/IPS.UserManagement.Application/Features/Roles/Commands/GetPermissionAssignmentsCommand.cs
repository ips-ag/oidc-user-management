using IPS.UserManagement.Application.Features.Permissions.Models;

namespace IPS.UserManagement.Application.Features.Roles.Commands;

public class GetPermissionAssignmentsCommand : IRequest<List<PermissionQueryModel>>
{
    private string RoleId { get; }

    public GetPermissionAssignmentsCommand(string roleId)
    {
        RoleId = roleId;
    }

    private class GetPermissionAssignmentsCommandHandler :
        IRequestHandler<GetPermissionAssignmentsCommand, List<PermissionQueryModel>>
    {
        public Task<List<PermissionQueryModel>> Handle(GetPermissionAssignmentsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
