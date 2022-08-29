using IPS.UserManagement.Application.Features.Resources.Converters;
using IPS.UserManagement.Application.Features.Resources.Models;
using IPS.UserManagement.Domain.Resources;

namespace IPS.UserManagement.Application.Features.Resources.Commands;

public class CreateResourceCommand : IRequest<ResourceQueryModel>
{
    private CreateResourceCommandModel Request { get; }

    public CreateResourceCommand(CreateResourceCommandModel request)
    {
        Request = request;
    }

    private class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand, ResourceQueryModel>
    {
        private readonly IResourceRepository _repository;
        private readonly ResourceRequestConverter _requestConverter;
        private readonly ResourceConverter _resourceConverter;

        public CreateResourceCommandHandler(
            IResourceRepository repository,
            ResourceRequestConverter requestConverter,
            ResourceConverter resourceConverter)
        {
            _repository = repository;
            _requestConverter = requestConverter;
            _resourceConverter = resourceConverter;
        }

        public async Task<ResourceQueryModel> Handle(CreateResourceCommand command, CancellationToken cancel)
        {
            var request = _requestConverter.ToDomain(command.Request);
            var resource = await _repository.CreateAsync(request, cancel);
            var model = _resourceConverter.ToModel(resource);
            return model;
        }
    }
}
