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
    private readonly RoleConverter _converter;
    private readonly IPermissionRepository _permissionRepository;

    public RoleRepository(
        UserManagementDbContext context,
        RoleConverter converter,
        IPermissionRepository permissionRepository)
    {
        _context = context;
        _converter = converter;
        _permissionRepository = permissionRepository;
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
        var role = _converter.ToDomain(model);
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
        if (model is null) throw new EntityNotFoundException(typeof(Role), id);
        var resource = _converter.ToDomain(model);
        return resource;
    }

    public async ValueTask<IReadOnlyCollection<Role>> GetAsync(CancellationToken cancel)
    {
        List<Role> roles = new();
        await foreach (var model in _context.Roles.AsAsyncEnumerable().WithCancellation(cancel))
        {
            var role = _converter.ToDomain(model);
            roles.Add(role);
        }
        return roles;
    }

    public async Task<Permission> AssignPermissionAsync(string id, string permissionId, CancellationToken cancel)
    {
        var roleModel = await GetRoleModelAsync(id, cancel);
        if (roleModel is null) throw new EntityNotFoundException(typeof(Role), id);
        var permission = await _permissionRepository.GetAsync(permissionId, cancel);
        
        // assign permission
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Permission>> GetPermissionsAsync(string id, CancellationToken cancel)
    {
        // get permission assignments
        // get permissions
        throw new NotImplementedException();
    }

    private async Task<RoleModel?> GetRoleModelAsync(string id, CancellationToken cancel)
    {
        var modelId = int.Parse(id);
        var model = await _context.Roles.Include(r => r.Permissions).SingleOrDefaultAsync(r => r.Id == modelId, cancel);
        return model;
    }
}
