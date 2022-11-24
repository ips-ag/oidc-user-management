using IPS.UserManagement.Application.Features.Permissions.Converters;
using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Domain.Permissions;

namespace IPS.UserManagement.Application.Features.Permissions.Commands;

public class GetPermissionsCommand : IRequest<List<PermissionQueryModel>>
{
    private string Resource { get; }

    public GetPermissionsCommand(string resource)
    {
        Resource = resource;
    }

    private class GetPermissionsCommandHandler : IRequestHandler<GetPermissionsCommand, List<PermissionQueryModel>>
    {
        private readonly IPermissionRepository _repository;
        private readonly PermissionConverter _converter;

        public GetPermissionsCommandHandler(IPermissionRepository repository, PermissionConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public async Task<List<PermissionQueryModel>> Handle(GetPermissionsCommand request, CancellationToken cancel)
        {
            var permissions = await _repository.GetByResourceAsync(request.Resource, cancel);
            var models = _converter.ToModel(permissions);
            return models;
        }
    }
}
