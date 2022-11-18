using System.Net.Http.Json;
using IPS.UserManagement.Application.Features.Resources.Models;
using IPS.UserManagement.Tests.Authentication;

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
        CreateResourceCommandModel commandModel = new()
        {
            Name = "ERP", Description = "ERP system for Contoso company", Location = "https://erp.contoso.com"
        };
        var content = JsonContent.Create(commandModel);
        var response = await client.PostAsync("resources", content, cancel);
        response.EnsureSuccessStatusCode();
        var model = await response.Content.ReadFromJsonAsync<ResourceQueryModel>(cancellationToken: cancel);
        Assert.NotNull(model);
        response = await client.GetAsync("resources", cancel);
        response.EnsureSuccessStatusCode();
        var models = await response.Content.ReadFromJsonAsync<List<ResourceQueryModel>>(cancellationToken: cancel);
        Assert.NotNull(models);
        Assert.NotEmpty(models);
    }
}
