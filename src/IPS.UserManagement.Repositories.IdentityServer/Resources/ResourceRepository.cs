using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using IPS.UserManagement.Domain.Resources;
using Microsoft.EntityFrameworkCore;

namespace IPS.UserManagement.Repositories.IdentityServer.Resources;

internal class ResourceRepository : IResourceRepository
{
    private readonly ConfigurationDbContext _dbContext;

    public ResourceRepository(ConfigurationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<Resource> CreateAsync(CreateRequest request, CancellationToken cancel)
    {
        ApiResource model = new()
        {
            Name = request.Name, DisplayName = request.Description, Description = request.Description
        };
        var savedModel = _dbContext.ApiResources.Add(model).Entity;
        await _dbContext.SaveChangesAsync(cancel);
        var resource = ToDomain(savedModel);
        return resource;
    }

    public async ValueTask<Resource?> GetAsync(string id, CancellationToken cancel)
    {
        int dbId = int.Parse(id);
        var model = await _dbContext.ApiResources.FirstOrDefaultAsync(r => r.Id == dbId, cancel);
        if (model is null) return null;
        var resource = ToDomain(model);
        return resource;
    }

    public async ValueTask<IReadOnlyCollection<Resource>> GetAsync(CancellationToken cancel)
    {
        List<Resource> resources = new();
        await foreach (var model in _dbContext.ApiResources.AsAsyncEnumerable().WithCancellation(cancel))
        {
            var resource = ToDomain(model);
            resources.Add(resource);
        }
        return resources;
    }

    private Resource ToDomain(ApiResource model)
    {
        return new Resource(
            model.Id.ToString("D"),
            model.Name,
            model.Description,
            model.DisplayName);
    }
}
