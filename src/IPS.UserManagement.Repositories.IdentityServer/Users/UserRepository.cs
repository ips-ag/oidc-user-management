using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using IPS.UserManagement.Domain.Exceptions;
using IPS.UserManagement.Domain.Roles;
using IPS.UserManagement.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace IPS.UserManagement.Repositories.IdentityServer.Users;

internal class UserRepository : IUserRepository, IAsyncDisposable
{
    private readonly ConfigurationDbContext _dbContext;
    private readonly IRoleRepository _roleRepository;

    public UserRepository(ConfigurationDbContext dbContext, IRoleRepository roleRepository)
    {
        _dbContext = dbContext;
        _roleRepository = roleRepository;
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask<Role> AssignRoleAsync(string id, string roleId, CancellationToken cancel)
    {
        var userModel = await GetUserModelAsync(id, cancel);
        var role = await _roleRepository.GetAsync(roleId, cancel);
        var permissions = await _roleRepository.GetAssignedPermissionsAsync(roleId, cancel);
        var existingScopes = userModel.AllowedScopes.Select(s => s.Scope).ToHashSet();
        foreach (var permission in permissions)
        {
            if (existingScopes.Contains(permission.Name)) continue;
            var scopeModel = new ClientScope { Scope = permission.Name };
            userModel.AllowedScopes.Add(scopeModel);
        }
        return role;
    }

    public ValueTask DeleteRoleAssignmentAsync(string id, string roleId, CancellationToken cancel)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<IReadOnlyCollection<Role>> GetAssignedRolesAsync(string id, CancellationToken cancel)
    {
        var userModel = await GetUserModelAsync(id, cancel);
        var roles = await _roleRepository.GetAsync(cancel);
        List<Role> assignedRoles = new();
        var existingScopes = userModel.AllowedScopes.Select(s => s.Scope).ToHashSet();
        foreach (var role in roles)
        {
            var permissions = await _roleRepository.GetAssignedPermissionsAsync(role.Id, cancel);
            if (permissions.All(p => existingScopes.Contains(p.Name)))
            {
                assignedRoles.Add(role);
            }
        }
        return assignedRoles;
    }

    private async Task<Client> GetUserModelAsync(string id, CancellationToken cancel)
    {
        var userModel = await _dbContext.Clients
            .Include(c => c.AllowedScopes)
            .SingleOrDefaultAsync(c => c.ClientId == id, cancel);
        if (userModel is null) throw new EntityNotFoundException(typeof(Client), id);
        return userModel;
    }
}
