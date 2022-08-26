namespace IPS.UserManagement.Domain.Resources;

public interface IResourceRepository
{
    ValueTask<Resource> CreateAsync(CreateRequest request, CancellationToken cancel);
    ValueTask<Resource?> GetAsync(string id, CancellationToken cancel);
    ValueTask<IReadOnlyCollection<Resource>> GetAsync(CancellationToken cancel);
}
