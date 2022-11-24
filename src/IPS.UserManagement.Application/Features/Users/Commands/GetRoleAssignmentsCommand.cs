using IPS.UserManagement.Application.Features.Roles.Converters;
using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Domain.Users;

namespace IPS.UserManagement.Application.Features.Users.Commands;

public class GetRoleAssignmentsCommand : IRequest<List<RoleQueryModel>>
{
    private string UserId { get; }

    public GetRoleAssignmentsCommand(string userId)
    {
        UserId = userId;
    }

    private class GetRoleAssignmentsCommandHandler : IRequestHandler<GetRoleAssignmentsCommand, List<RoleQueryModel>>
    {
        private readonly IUserRepository _repository;
        private readonly RoleConverter _converter;

        public GetRoleAssignmentsCommandHandler(IUserRepository repository, RoleConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public async Task<List<RoleQueryModel>> Handle(GetRoleAssignmentsCommand command, CancellationToken cancel)
        {
            var models = await _repository.GetAssignedRolesAsync(command.UserId, cancel);
            var roles = _converter.ToModel(models);
            return roles;
        }
    }
}
