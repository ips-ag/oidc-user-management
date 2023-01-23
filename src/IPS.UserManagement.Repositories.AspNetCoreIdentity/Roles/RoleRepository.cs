using IPS.UserManagement.Domain.Exceptions;
using IPS.UserManagement.Domain.Permissions;
using IPS.UserManagement.Domain.Roles;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.DbContexts;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.Roles.Converters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CreateRequest = IPS.UserManagement.Domain.Roles.CreateRequest;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.Roles;

internal class RoleRepository : IRoleRepository, IAsyncDisposable
{
    private readonly RoleManager<ApplicationRoleModel> _roleManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleConverter _roleConverter;
    private readonly IPermissionRepository _permissionRepository;
    private readonly PermissionConverter _permissionConverter;

    public RoleRepository(
        RoleManager<ApplicationRoleModel> roleManager,
        RoleConverter roleConverter,
        PermissionConverter permissionConverter,
        ApplicationDbContext dbContext,
        IPermissionRepository permissionRepository)
    {
        _roleManager = roleManager;
        _roleConverter = roleConverter;
        _permissionConverter = permissionConverter;
        _dbContext = dbContext;
        _permissionRepository = permissionRepository;
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask<Role> CreateAsync(CreateRequest request, CancellationToken cancel)
    {
        var model = await _roleManager.FindByNameAsync(request.Name);
        if (model is not null) return _roleConverter.ToDomain(model);
        model = _roleConverter.ToModel(request);
        var result = await _roleManager.CreateAsync(model);
        if (!result.Succeeded)
        {
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        return _roleConverter.ToDomain(model);
    }

    public async ValueTask DeleteAsync(string id, CancellationToken cancel)
    {
        await _roleManager.DeleteAsync(new ApplicationRoleModel { Id = id });
    }

    public async ValueTask<Role> GetAsync(string id, CancellationToken cancel)
    {
        var model = await GetRoleModelAsync(id, cancel);
        return _roleConverter.ToDomain(model);
    }

    public async ValueTask<IReadOnlyCollection<Role>> GetAsync(CancellationToken cancel)
    {
        List<Role> roles = new();
        await foreach (var model in _roleManager.Roles.AsAsyncEnumerable().WithCancellation(cancel))
        {
            var role = _roleConverter.ToDomain(model);
            roles.Add(role);
        }
        return roles;
    }

    public async ValueTask<IReadOnlyCollection<Permission>> GetAssignedPermissionsAsync(
        string id,
        CancellationToken cancel)
    {
        // assert exists
        await GetRoleModelAsync(id, cancel);
        List<Permission> permissions = new();
        await foreach (var model in _dbContext.RoleClaims
                           .Where(c => c.RoleId == id)
                           .AsAsyncEnumerable()
                           .WithCancellation(cancel))
        {
            var permission = await _permissionRepository.GetByNameAsync(model.ClaimType, cancel);
            permissions.Add(permission);
        }
        return permissions;
    }

    public async ValueTask<Permission> AssignPermissionAsync(
        string id,
        Permission permission,
        CancellationToken cancel)
    {
        var claimModel = await _dbContext.RoleClaims.SingleOrDefaultAsync(
            c => c.RoleId == id && c.ClaimType == permission.Name,
            cancel);
        if (claimModel is not null) return permission;
        claimModel = _permissionConverter.ToModel(id, permission);
        _dbContext.RoleClaims.Add(claimModel);
        return permission;
    }

    private async Task<ApplicationRoleModel> GetRoleModelAsync(string id, CancellationToken cancel)
    {
        var model = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Id == id, cancel);
        if (model is null) throw new EntityNotFoundException(typeof(Role), id);
        return model;
    }
}
