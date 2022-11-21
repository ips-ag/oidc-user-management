using IPS.UserManagement.Application.Features.Resources.Models;
using IPS.UserManagement.Tests.Authentication;
using IPS.UserManagement.Tests.Resources;

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
    public async Task ShouldCreateResource()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;
        var client = _fixture.UserManagementClient;
        await client.LoginAsync(
            _fixture.IdentityServerClient,
            cancel,
            scope: "resources:full");
        CreateResourceCommandModel command = new()
        {
            Name = "ERP", Description = "ERP system for Contoso company", Location = "https://erp.contoso.com"
        };
        var model = await client.CreateResourceAsync(command, cancel);
        var models = await client.GetResourcesAsync(cancel);
        Assert.Contains(models, m => m.Id == model.Id);
    }
}
