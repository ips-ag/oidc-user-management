using System.Collections.Concurrent;
using IPS.UserManagement.Domain.Resources;

namespace IPS.UserManagement.Repositories.Memory.Resources;

internal class ResourceRepository : IResourceRepository
{
    private readonly ConcurrentDictionary<string, Resource> _resources = new();

    public ValueTask<Resource> CreateAsync(CreateRequest request, CancellationToken cancel)
    {
        var id = Guid.NewGuid().ToString("N");
        Resource resource = new(id, request.Name, request.Description, request.Location);
        _resources.TryAdd(id, resource);
        return ValueTask.FromResult(resource);
    }

    public ValueTask<Resource?> GetAsync(string id, CancellationToken cancel)
    {
        var result = _resources.TryGetValue(id, out var resource) ? resource : null;
        return ValueTask.FromResult(result);
    }

    public async ValueTask<IReadOnlyCollection<Resource>> GetAsync(CancellationToken cancel)
    {
        await ValueTask.CompletedTask;
        return _resources.Values.ToList();
    }
}
