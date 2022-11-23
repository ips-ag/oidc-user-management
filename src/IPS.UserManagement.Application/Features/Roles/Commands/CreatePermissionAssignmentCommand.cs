using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Application.Features.Roles.Models;

namespace IPS.UserManagement.Application.Features.Roles.Commands;

public class CreatePermissionAssignmentCommand: IRequest<PermissionQueryModel>
{
    private string RoleId { get; }
    private CreatePermissionAssignmentCommandModel Request { get; }

    public CreatePermissionAssignmentCommand(string roleId, CreatePermissionAssignmentCommandModel request)
    {
        RoleId = roleId;
        Request = request;
    }

    private class CreatePermissionAssignmentCommandHandler : 
        IRequestHandler<CreatePermissionAssignmentCommand, PermissionQueryModel>
    {
        public Task<PermissionQueryModel> Handle(CreatePermissionAssignmentCommand command, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }
    }
}
