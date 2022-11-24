using IPS.UserManagement.Application.Features.Permissions.Converters;
using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Domain.Roles;

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
        private readonly IRoleRepository _roleRepository;
        private readonly PermissionConverter _permissionConverter;

        public GetPermissionAssignmentsCommandHandler(
            IRoleRepository roleRepository,
            PermissionConverter permissionConverter)
        {
            _roleRepository = roleRepository;
            _permissionConverter = permissionConverter;
        }

        public async Task<List<PermissionQueryModel>> Handle(
            GetPermissionAssignmentsCommand command,
            CancellationToken cancel)
        {
            var permissions = await _roleRepository.GetAssignedPermissionsAsync(command.RoleId, cancel);
            return _permissionConverter.ToModel(permissions);
        }
    }
}
