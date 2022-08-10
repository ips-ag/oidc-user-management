using System.Net.Http.Json;
using IPS.UserManagement.Application.Features.Resources.Models;

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
        CreateResourceCommandModel commandModel = new();
        var content = JsonContent.Create(commandModel);
        var response = await _fixture.Client.PostAsync("resources", content);
        response.EnsureSuccessStatusCode();
        var model = response.Content.ReadFromJsonAsync<ResourceQueryModel>();
        Assert.NotNull(model);
    }
}
