using IPS.UserManagement.Domain.Permissions;

namespace IPS.UserManagement.Domain.Roles;

public interface IRoleRepository
{
    ValueTask<Role> CreateAsync(CreateRequest request, CancellationToken cancel);
    ValueTask DeleteAsync(string id, CancellationToken cancel);
    ValueTask<Role> GetAsync(string id, CancellationToken cancel);
    ValueTask<IReadOnlyCollection<Role>> GetAsync(CancellationToken cancel);
    Task<IReadOnlyCollection<Permission>> GetPermissionsAsync(string id, CancellationToken cancel);
    Task<Permission> AssignPermissionAsync(string id, string permissionId, CancellationToken cancel);
}
