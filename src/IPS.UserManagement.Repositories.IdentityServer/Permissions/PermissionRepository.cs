using IPS.UserManagement.Domain.Permissions;

namespace IPS.UserManagement.Repositories.IdentityServer.Permissions;

public class PermissionRepository: IPermissionRepository
{
    public ValueTask<Permission> CreateAsync(CreateRequest request, CancellationToken cancel)
    {
        throw new NotImplementedException();
    }

    public ValueTask DeleteAsync(DeleteRequest request, CancellationToken cancel)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Permission> GetAsync(string id, CancellationToken cancel)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IReadOnlyCollection<Permission>> GetByResourceAsync(string resource, CancellationToken cancel)
    {
        throw new NotImplementedException();
    }
}
