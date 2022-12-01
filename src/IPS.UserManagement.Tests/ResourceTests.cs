using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Application.Features.Resources.Models;
using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Application.Features.Users.Models;
using IPS.UserManagement.Tests.Authentication;
using IPS.UserManagement.Tests.Permissions;
using IPS.UserManagement.Tests.Resources;
using IPS.UserManagement.Tests.Roles;
using IPS.UserManagement.Tests.Users;

namespace IPS.UserManagement.Tests;

[Collection(CollectionNames.Default)]
public class ResourceTests
{
    private readonly HostFixture _fixture;

    public ResourceTests(HostFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _fixture.TestOutputHelper = output;
    }

    [Fact]
    public async Task ShouldCreateResourceAndPermitAccess()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;
        var client = _fixture.UserManagementClient;
        await client.LoginAsync(
            _fixture.IdentityServerClient,
            cancel,
            scope:
            "resources:full permissions:full roles:full users:read permission-assignments:full role-assignments:full");
        // create new resource
        CreateResourceCommandModel resourceCommand = new()
        {
            Name = "ERP", Description = "ERP system for Contoso company", Location = "https://erp.contoso.com"
        };
        var resource = await client.CreateResourceAsync(resourceCommand, cancel);
        var resources = await client.GetResourcesAsync(cancel);
        Assert.Contains(resources, m => m.Id == resource.Id);
        // create new resource permission
        CreatePermissionCommandModel permissionCommand = new()
        {
            Name = "orders:read", Description = "Read all orders", Resource = resource.Id
        };
        var permission = await client.CreatePermissionAsync(permissionCommand, cancel);
        var permissions = await client.GetPermissionsForResourceAsync(resource.Id, cancel);
        Assert.Contains(permissions, p => p.Id == permission.Id);
        // create new role
        CreateRoleCommandModel roleCommand = new() { Name = "Order Supervisor", Description = "Supervises all orders" };
        var role = await client.CreateRoleAsync(roleCommand, cancel);
        var roles = await client.GetRolesAsync(cancel);
        Assert.Contains(roles, r => r.Id == role.Id);
        // assign permission to role
        CreatePermissionAssignmentCommandModel permissionAssignmentCommand = new() { Permission = permission.Id };
        var permissionAssignment = await client.AssignPermissionAsync(role.Id, permissionAssignmentCommand, cancel);
        var permissionAssignments = await client.GetPermissionsForRoleAsync(role.Id, cancel);
        Assert.Contains(permissionAssignments, p => p.Id == permissionAssignment.Id);
        // assign role to user
        var userName = "bob";
        var users = await client.GetUsersAsync(null, userName, cancel);
        var user = Assert.Single(users, u => u.UserName == userName);
        var userId = user.Id;
        CreateRoleAssignmentCommandModel roleAssignmentCommand = new() { RoleId = role.Id };
        var roleAssignment = await client.AssignRoleAsync(userId, roleAssignmentCommand, cancel);
        var roleAssignments = await client.GetRolesForUserAsync(userId, cancel);
        Assert.Contains(roleAssignments, p => p.Id == roleAssignment.Id);
        // access new resource as user
        var erpClient = _fixture.ErpClient;
        await erpClient.LoginUserAsync(
            _fixture.IdentityServerClient,
            userName: userName,
            password: "secret",
            scope: permission.Name,
            clientId: "erp",
            cancel: cancel);
        var erpResponse = await erpClient.GetAsync("orders", cancel);
        erpResponse.EnsureSuccessStatusCode();
    }
}
