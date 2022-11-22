using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Application.Features.Resources.Models;
using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Tests.Authentication;
using IPS.UserManagement.Tests.Permissions;
using IPS.UserManagement.Tests.Resources;
using IPS.UserManagement.Tests.Roles;

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
            scope: "resources:full permissions:full roles:full permission-assignments:full role-assignments:full");
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
        CreateRoleCommandModel roleCommand = new();
        var role = await client.CreateRoleAsync(roleCommand, cancel);
        var roles = await client.GetRolesAsync(cancel);
        Assert.Contains(roles, r => r.Id == role.Id);
        // assign permission to role
        // assign role to user
        // access new resource as user
    }
}
