using IPS.UserManagement.Domain.Roles;

namespace IPS.UserManagement.Domain.Users;

public interface IUserRepository
{
    ValueTask<Role> AssignRoleAsync(string id, string roleId, CancellationToken cancel);
    ValueTask DeleteRoleAssignmentAsync(string id, string roleId, CancellationToken cancel);
    ValueTask<IReadOnlyCollection<Role>> GetAssignedRolesAsync(string id, CancellationToken cancel);
}
