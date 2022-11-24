using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using IPS.UserManagement.Domain.Exceptions;
using IPS.UserManagement.Domain.Resources;
using IPS.UserManagement.Repositories.IdentityServer.Resources.Converters;
using Microsoft.EntityFrameworkCore;

namespace IPS.UserManagement.Repositories.IdentityServer.Resources;

internal class ResourceRepository : IResourceRepository, IAsyncDisposable
{
    private readonly ConfigurationDbContext _dbContext;
    private readonly ResourceConverter _converter;

    public ResourceRepository(ConfigurationDbContext dbContext, ResourceConverter converter)
    {
        _dbContext = dbContext;
        _converter = converter;
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask<Resource> CreateAsync(CreateRequest request, CancellationToken cancel)
    {
        var model = await _dbContext.ApiResources.Where(i => i.Name == request.Name).SingleOrDefaultAsync(cancel);
        if (model is null)
        {
            // create
            model = new ApiResource
            {
                Name = request.Name,
                DisplayName = request.Location,
                Description = request.Description,
                Scopes = new List<ApiResourceScope>()
            };
            model = _dbContext.ApiResources.Add(model).Entity;
        }
        else
        {
            // update
            model.DisplayName = request.Location;
            model.Description = request.Description;
            _dbContext.ApiResources.Update(model);
        }
        await _dbContext.SaveChangesAsync(cancel);
        var resource = _converter.ToDomain(model);
        return resource;
    }

    public async ValueTask DeleteAsync(string id, CancellationToken cancel)
    {
        var modelId = int.Parse(id);
        var model = await _dbContext.ApiResources.FindAsync(new object?[] { modelId }, cancel);
        if (model is null) return;
        _dbContext.ApiResources.Remove(model);
    }

    public async ValueTask<Resource> GetAsync(string id, CancellationToken cancel)
    {
        var modelId = int.Parse(id);
        var model = await _dbContext.ApiResources.FindAsync(new object?[] { modelId }, cancel);
        if (model is null) throw new EntityNotFoundException(typeof(Resource), id);
        var resource = _converter.ToDomain(model);
        return resource;
    }

    public async ValueTask<IReadOnlyCollection<Resource>> GetAsync(CancellationToken cancel)
    {
        List<Resource> resources = new();
        await foreach (var model in _dbContext.ApiResources.AsAsyncEnumerable().WithCancellation(cancel))
        {
            var resource = _converter.ToDomain(model);
            resources.Add(resource);
        }
        return resources;
    }
}
