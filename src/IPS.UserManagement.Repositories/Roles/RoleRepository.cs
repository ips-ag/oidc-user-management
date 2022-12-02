using IPS.UserManagement.Domain.Exceptions;
using IPS.UserManagement.Domain.Permissions;
using IPS.UserManagement.Domain.Roles;
using IPS.UserManagement.Repositories.EntityFramework.DbContexts;
using IPS.UserManagement.Repositories.EntityFramework.Models;
using IPS.UserManagement.Repositories.Roles.Converters;
using Microsoft.EntityFrameworkCore;
using CreateRequest = IPS.UserManagement.Domain.Roles.CreateRequest;

namespace IPS.UserManagement.Repositories.Roles;

internal class RoleRepository : IRoleRepository, IAsyncDisposable
{
    private readonly UserManagementDbContext _context;
    private readonly RoleConverter _roleConverter;
    private readonly RolePermissionConverter _permissionConverter;
    private readonly IPermissionRepository _permissionRepository;

    public RoleRepository(
        UserManagementDbContext context,
        RoleConverter roleConverter,
        RolePermissionConverter permissionConverter,
        IPermissionRepository permissionRepository)
    {
        _context = context;
        _roleConverter = roleConverter;
        _permissionRepository = permissionRepository;
        _permissionConverter = permissionConverter;
    }

    public async ValueTask DisposeAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async ValueTask<Role> CreateAsync(CreateRequest request, CancellationToken cancel)
    {
        var model = await _context.Roles.Where(i => i.Name == request.Name).SingleOrDefaultAsync(cancel);
        if (model is null)
        {
            // create
            model = new RoleModel { Name = request.Name, Description = request.Description };
            model = _context.Roles.Add(model).Entity;
        }
        else
        {
            // update
            model.Description = request.Description;
            _context.Roles.Update(model);
        }
        await _context.SaveChangesAsync(cancel);
        var role = _roleConverter.ToDomain(model);
        return role;
    }

    public ValueTask DeleteAsync(string id, CancellationToken cancel)
    {
        var model = new RoleModel { Id = int.Parse(id) };
        _context.Remove(model);
        return ValueTask.CompletedTask;
    }

    public async ValueTask<Role> GetAsync(string id, CancellationToken cancel)
    {
        var model = await GetRoleModelAsync(id, cancel);
        var resource = _roleConverter.ToDomain(model);
        return resource;
    }

    public async ValueTask<IReadOnlyCollection<Role>> GetAsync(CancellationToken cancel)
    {
        List<Role> roles = new();
        await foreach (var model in _context.Roles.AsAsyncEnumerable().WithCancellation(cancel))
        {
            var role = _roleConverter.ToDomain(model);
            roles.Add(role);
        }
        return roles;
    }

    public async ValueTask<Permission> AssignPermissionAsync(
        string id,
        Permission permission,
        CancellationToken cancel)
    {
        var roleModel = await GetRoleModelAsync(id, cancel);
        var permissionModel = _permissionConverter.ToModel(permission);
        roleModel.Permissions.Add(permissionModel);
        _context.Roles.Update(roleModel);
        return permission;
    }

    public async ValueTask<IReadOnlyCollection<Permission>> GetAssignedPermissionsAsync(
        string id,
        CancellationToken cancel)
    {
        var roleModel = await GetRoleModelAsync(id, cancel);
        var permissionIds = roleModel.Permissions.Select(p => p.PermissionId).ToList();
        var permissions = await _permissionRepository.GetAsync(permissionIds, cancel);
        return permissions;
    }

    private async Task<RoleModel> GetRoleModelAsync(string id, CancellationToken cancel)
    {
        var modelId = int.Parse(id);
        var model = await _context.Roles.Include(r => r.Permissions).SingleOrDefaultAsync(r => r.Id == modelId, cancel);
        if (model is null) throw new EntityNotFoundException(typeof(Role), id);
        return model;
    }
}
