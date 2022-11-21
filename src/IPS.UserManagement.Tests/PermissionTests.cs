using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Application.Features.Resources.Models;
using IPS.UserManagement.Tests.Authentication;
using IPS.UserManagement.Tests.Permissions;
using IPS.UserManagement.Tests.Resources;

namespace IPS.UserManagement.Tests;

[Collection(CollectionNames.Default)]
public class PermissionTests
{
    private readonly HostFixture _fixture;

    public PermissionTests(HostFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _fixture.TestOutputHelper = output;
    }

    [Fact]
    public async Task ShouldCreatePermission()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;
        var client = _fixture.UserManagementClient;
        await client.LoginAsync(
            _fixture.IdentityServerClient,
            cancel,
            scope: "resources:full permissions:full roles:full permission-assignments:full role-assignments:full");
        CreateResourceCommandModel resourceCommand = new()
        {
            Name = "ERP", Description = "ERP system for Contoso company", Location = "https://erp.contoso.com"
        };
        var resource = await client.CreateResourceAsync(resourceCommand, cancel);
        CreatePermissionCommandModel permissionCommand = new()
        {
            Name = "orders:read", Description = "Read all orders", Resource = resource.Id
        };
        var permission = await client.CreatePermissionAsync(permissionCommand, cancel);
        var permissions = await client.GetPermissionsForResourceAsync(resource.Id, cancel);
        Assert.Contains(permissions, p => p.Id == permission.Id);
    }
}
