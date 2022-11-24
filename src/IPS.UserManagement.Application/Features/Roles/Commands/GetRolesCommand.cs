using IPS.UserManagement.Application.Features.Roles.Converters;
using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Domain.Roles;

namespace IPS.UserManagement.Application.Features.Roles.Commands;

public class GetRolesCommand : IRequest<List<RoleQueryModel>>
{
    private class GetRolesCommandHandler : IRequestHandler<GetRolesCommand, List<RoleQueryModel>>
    {
        private readonly IRoleRepository _repository;
        private readonly RoleConverter _converter;

        public GetRolesCommandHandler(IRoleRepository repository, RoleConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public async Task<List<RoleQueryModel>> Handle(GetRolesCommand request, CancellationToken cancel)
        {
            var roles = await _repository.GetAsync(cancel);
            return _converter.ToModel(roles);
        }
    }
}
