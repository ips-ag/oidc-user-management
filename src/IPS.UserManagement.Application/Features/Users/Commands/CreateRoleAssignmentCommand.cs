using IPS.UserManagement.Application.Features.Roles.Converters;
using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Application.Features.Users.Models;
using IPS.UserManagement.Domain.Users;

namespace IPS.UserManagement.Application.Features.Users.Commands;

public class CreateRoleAssignmentCommand : IRequest<RoleQueryModel>
{
    private string UserId { get; }
    private CreateRoleAssignmentCommandModel Request { get; }

    public CreateRoleAssignmentCommand(string userId, CreateRoleAssignmentCommandModel request)
    {
        UserId = userId;
        Request = request;
    }

    private class CreateRoleAssignmentCommandHandler : IRequestHandler<CreateRoleAssignmentCommand, RoleQueryModel>
    {
        private readonly IUserRepository _repository;
        private readonly RoleConverter _converter;

        public CreateRoleAssignmentCommandHandler(IUserRepository repository, RoleConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public async Task<RoleQueryModel> Handle(CreateRoleAssignmentCommand command, CancellationToken cancel)
        {
            var model = await _repository.AssignRoleAsync(command.UserId, command.Request.RoleId, cancel);
            var role = _converter.ToModel(model);
            return role;
        }
    }
}
