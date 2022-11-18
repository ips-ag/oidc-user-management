namespace IPS.UserManagement.Domain.Permissions;

public interface IPermissionRepository
{
    ValueTask<Permission> CreateAsync(CreateRequest request, CancellationToken cancel);
    ValueTask DeleteAsync(DeleteRequest request, CancellationToken cancel);
    ValueTask<Permission> GetAsync(string id, CancellationToken cancel);
    ValueTask<IReadOnlyCollection<Permission>> GetByResourceAsync(string resource, CancellationToken cancel);
}
