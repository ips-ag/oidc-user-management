using IPS.UserManagement.Application.Features.Resources.Converters;
using IPS.UserManagement.Application.Features.Resources.Models;
using IPS.UserManagement.Domain.Resources;

namespace IPS.UserManagement.Application.Features.Resources.Commands;

public class GetResourcesCommand : IRequest<List<ResourceQueryModel>>
{
    private class GetResourcesCommandHandler : IRequestHandler<GetResourcesCommand, List<ResourceQueryModel>>
    {
        private readonly IResourceRepository _repository;
        private readonly ResourceConverter _converter;

        public GetResourcesCommandHandler(IResourceRepository repository, ResourceConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public async Task<List<ResourceQueryModel>> Handle(GetResourcesCommand request, CancellationToken cancel)
        {
            var resources = await _repository.GetAsync(cancel);
            var models = _converter.ToModel(resources);
            return models;
        }
    }
}
