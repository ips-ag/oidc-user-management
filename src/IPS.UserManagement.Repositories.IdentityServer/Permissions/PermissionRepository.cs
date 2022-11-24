using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using IPS.UserManagement.Domain.Exceptions;
using IPS.UserManagement.Domain.Permissions;
using IPS.UserManagement.Domain.Resources;
using IPS.UserManagement.Repositories.IdentityServer.Permissions.Converters;
using Microsoft.EntityFrameworkCore;
using CreateRequest = IPS.UserManagement.Domain.Permissions.CreateRequest;

namespace IPS.UserManagement.Repositories.IdentityServer.Permissions;

internal class PermissionRepository : IPermissionRepository, IAsyncDisposable
{
    private readonly ConfigurationDbContext _dbContext;
    private readonly PermissionConverter _converter;

    public PermissionRepository(ConfigurationDbContext dbContext, PermissionConverter converter)
    {
        _dbContext = dbContext;
        _converter = converter;
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask<Permission> CreateAsync(CreateRequest request, CancellationToken cancel)
    {
        var resourceModel = await GetResourceModelAsync(request.Resource, cancel);
        var scopeModel = await _dbContext.ApiScopes
            .Where(i => i.Name == request.Name)
            .SingleOrDefaultAsync(cancel);
        if (scopeModel is null)
        {
            // create
            scopeModel = new ApiScope
            {
                Name = request.Name, DisplayName = request.Description, Description = request.Description
            };
            scopeModel = _dbContext.ApiScopes.Add(scopeModel).Entity;
            var apiResourceScopeModel = new ApiResourceScope { Scope = request.Name };
            resourceModel.Scopes.Add(apiResourceScopeModel);
            _dbContext.ApiResources.Update(resourceModel);
        }
        else
        {
            // update
            scopeModel.DisplayName = request.Description;
            scopeModel.Description = request.Description;
            _dbContext.ApiScopes.Update(scopeModel);
        }
        await _dbContext.SaveChangesAsync(cancel);
        var permission = _converter.ToDomain(scopeModel);
        return permission;
    }

    public async ValueTask DeleteAsync(DeleteRequest request, CancellationToken cancel)
    {
        var resourceModel = await GetResourceModelAsync(request.Resource, cancel);
        if (resourceModel is null) throw new EntityNotFoundException(typeof(Resource), request.Resource);
        var modelId = int.Parse(request.Id);
        var model = await _dbContext.ApiScopes.FindAsync(new object?[] { modelId }, cancel);
        if (model is null) return;
        _dbContext.ApiScopes.Remove(model);
    }

    public async ValueTask<Permission> GetAsync(string id, CancellationToken cancel)
    {
        var modelId = int.Parse(id);
        var model = await _dbContext.ApiScopes.FindAsync(new object?[] { modelId }, cancel);
        if (model is null) throw new EntityNotFoundException(typeof(Permission), id);
        var permission = _converter.ToDomain(model);
        return permission;
    }

    public async ValueTask<IReadOnlyCollection<Permission>> GetAsync(
        IReadOnlyCollection<string> ids,
        CancellationToken cancel)
    {
        var modelIds = ids.Select(int.Parse).ToHashSet();
        List<Permission> permissions = new();
        await foreach (var model in _dbContext.ApiScopes
                           .Where(s => modelIds.Contains(s.Id))
                           .AsAsyncEnumerable()
                           .WithCancellation(cancel))
        {
            var permission = _converter.ToDomain(model);
            permissions.Add(permission);
        }
        return permissions;
    }

    public async ValueTask<IReadOnlyCollection<Permission>> GetByResourceAsync(
        string resourceId,
        CancellationToken cancel)
    {
        List<Permission> permissions = new();
        var resourceModel = await GetResourceModelAsync(resourceId, cancel);
        var scopeNames = resourceModel.Scopes.Select(s => s.Scope).ToHashSet();
        await foreach (var model in _dbContext.ApiScopes
                           .Where(s => scopeNames.Contains(s.Name))
                           .AsAsyncEnumerable()
                           .WithCancellation(cancel))
        {
            var permission = _converter.ToDomain(model);
            permissions.Add(permission);
        }
        return permissions;
    }

    private async Task<ApiResource> GetResourceModelAsync(string id, CancellationToken cancel)
    {
        var modelId = int.Parse(id);
        var resourceModel = await _dbContext.ApiResources
            .Include(r => r.Scopes)
            .SingleOrDefaultAsync(r => r.Id == modelId, cancel);
        if (resourceModel is null) throw new EntityNotFoundException(typeof(Resource), id);
        return resourceModel;
    }
}
