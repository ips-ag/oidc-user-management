using IPS.UserManagement.Domain.Exceptions;
using IPS.UserManagement.Domain.Roles;
using IPS.UserManagement.Domain.Users;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.Roles.Converters;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.Users.Converters;
using Microsoft.AspNetCore.Identity;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.Users;

internal class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUserModel> _userManager;
    private readonly RoleManager<ApplicationRoleModel> _roleManager;
    private readonly RoleConverter _roleConverter;
    private readonly UserConverter _userConverter;

    public UserRepository(
        UserManager<ApplicationUserModel> userManager,
        RoleManager<ApplicationRoleModel> roleManager,
        RoleConverter roleConverter,
        UserConverter userConverter)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _roleConverter = roleConverter;
        _userConverter = userConverter;
    }

    public async ValueTask<Role> AssignRoleAsync(string id, string roleId, CancellationToken cancel)
    {
        var userModel = await GetUserModelAsync(id);
        var roleModel = await GetRoleModelAsync(roleId);
        var result = await _userManager.AddToRoleAsync(userModel, roleModel.Name);
        if (!result.Succeeded)
        {
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        return _roleConverter.ToDomain(roleModel);
    }

    public async ValueTask DeleteRoleAssignmentAsync(string id, string roleId, CancellationToken cancel)
    {
        var userModel = await GetUserModelAsync(id);
        var roleModel = await GetRoleModelAsync(roleId);
        await _userManager.RemoveFromRoleAsync(userModel, roleModel.Name);
    }

    public async ValueTask<IReadOnlyCollection<Role>> GetAssignedRolesAsync(string id, CancellationToken cancel)
    {
        var userModel = await GetUserModelAsync(id);
        var roleNames = await _userManager.GetRolesAsync(userModel);
        List<Role> roles = new();
        foreach (var roleName in roleNames)
        {
            var roleModel = await _roleManager.FindByNameAsync(roleName);
            if (roleModel is null) continue;
            var role = _roleConverter.ToDomain(roleModel);
            roles.Add(role);
        }
        return roles;
    }

    public async ValueTask<IReadOnlyCollection<User>> QueryAsync(QueryRequest request, CancellationToken cancel)
    {
        Dictionary<string, User> users = new();
        if (request.Id is not null)
        {
            var model = await _userManager.FindByIdAsync(request.Id);
            if (model is not null)
            {
                var user = _userConverter.ToDomain(model);
                users[user.Id] = user;
            }
        }
        if (request.UserName is not null)
        {
            var model = await _userManager.FindByNameAsync(request.UserName);
            if (model is not null)
            {
                var user = _userConverter.ToDomain(model);
                users[user.Id] = user;
            }
        }
        return users.Values;
    }

    private async Task<ApplicationUserModel> GetUserModelAsync(string id)
    {
        var model = await _userManager.FindByIdAsync(id);
        if (model is null) throw new EntityNotFoundException(typeof(ApplicationUserModel), id);
        return model;
    }

    private async Task<ApplicationRoleModel> GetRoleModelAsync(string id)
    {
        var model = await _roleManager.FindByIdAsync(id);
        if (model is null) throw new EntityNotFoundException(typeof(ApplicationRoleModel), id);
        return model;
    }
}
