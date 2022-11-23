using IPS.UserManagement.Application.Features.Roles.Converters;
using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Domain.Roles;

namespace IPS.UserManagement.Application.Features.Roles.Commands;

public class CreateRoleCommand : IRequest<RoleQueryModel>
{
    private CreateRoleCommandModel Request { get; }

    public CreateRoleCommand(CreateRoleCommandModel request)
    {
        Request = request;
    }

    private class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleQueryModel>
    {
        private readonly IRoleRepository _repository;
        private readonly RoleRequestConverter _requestConverter;
        private readonly RoleConverter _converter;

        public CreateRoleCommandHandler(
            IRoleRepository repository,
            RoleRequestConverter requestConverter,
            RoleConverter converter)
        {
            _repository = repository;
            _requestConverter = requestConverter;
            _converter = converter;
        }

        public async Task<RoleQueryModel> Handle(CreateRoleCommand command, CancellationToken cancel)
        {
            var request = _requestConverter.ToDomain(command.Request);
            var role = await _repository.CreateAsync(request, cancel);
            return _converter.ToModel(role);
        }
    }
}
