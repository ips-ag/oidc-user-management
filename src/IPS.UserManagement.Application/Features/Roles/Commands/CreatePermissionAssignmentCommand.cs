using IPS.UserManagement.Application.Features.Permissions.Converters;
using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Domain.Permissions;
using IPS.UserManagement.Domain.Roles;

namespace IPS.UserManagement.Application.Features.Roles.Commands;

public class CreatePermissionAssignmentCommand : IRequest<PermissionQueryModel>
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
        private readonly IPermissionRepository _permissionRepository;
        private readonly PermissionConverter _permissionConverter;

        public CreatePermissionAssignmentCommandHandler(
            IRoleRepository repository,
            PermissionConverter permissionConverter,
            IPermissionRepository permissionRepository)
        {
            _repository = repository;
            _permissionConverter = permissionConverter;
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionQueryModel> Handle(
            CreatePermissionAssignmentCommand command,
            CancellationToken cancel)
        {
            var permission = await _permissionRepository.GetAsync(command.Request.Permission, cancel);
            await _repository.AssignPermissionAsync(
                command.RoleId,
                permission,
                cancel);
            return _permissionConverter.ToModel(permission);
        }
    }
}
