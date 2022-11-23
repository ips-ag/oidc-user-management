using IPS.UserManagement.Application.Features.Permissions.Converters;
using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Domain.Roles;

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
        private readonly IRoleRepository _repository;
        private readonly PermissionConverter _permissionConverter;

        public CreatePermissionAssignmentCommandHandler(IRoleRepository repository, PermissionConverter permissionConverter)
        {
            _repository = repository;
            _permissionConverter = permissionConverter;
        }

        public async Task<PermissionQueryModel> Handle(CreatePermissionAssignmentCommand command, CancellationToken cancel)
        {
            var permission = await _repository.AssignPermissionAsync(
                command.RoleId,
                command.Request.Permission,
                cancel);
            return _permissionConverter.ToModel(permission);
        }
    }
}
