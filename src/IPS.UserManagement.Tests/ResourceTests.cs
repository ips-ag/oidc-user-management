using System.Net.Http.Json;
using IdentityModel.Client;
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
        var token = new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;
        var client = _fixture.Client;
        var disco = await client.GetDiscoveryDocumentAsync(cancellationToken: token);
        Assert.False(disco.IsError, disco.Error);
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "resources:full"
            },
            cancellationToken: token);
        Assert.False(tokenResponse.IsError, tokenResponse.Error);
        client.SetBearerToken(tokenResponse.AccessToken);
        CreateResourceCommandModel commandModel = new();
        var content = JsonContent.Create(commandModel);
        var response = await client.PostAsync("resources", content, token);
        response.EnsureSuccessStatusCode();
        var model = response.Content.ReadFromJsonAsync<ResourceQueryModel>(cancellationToken: token);
        Assert.NotNull(model);
    }
}
