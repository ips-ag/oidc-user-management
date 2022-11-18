using System.Net.Http.Json;
using IPS.UserManagement.Application.Features.Resources.Models;
using IPS.UserManagement.Tests.Authentication;

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
        CreatePermissionCommandModel commandModel = new()
        {
            Name = "orders:read", Description = "Read all orders", Resource = "erpResourceId"
        };
        var content = JsonContent.Create(commandModel);
        var response = await client.PostAsync("permissions", content, cancel);
        response.EnsureSuccessStatusCode();
    }
}
