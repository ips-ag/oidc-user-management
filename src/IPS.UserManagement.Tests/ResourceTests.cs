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
        var cancel = new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;
        var client = _fixture.Client;
        var disco = await client.GetDiscoveryDocumentAsync(cancellationToken: cancel);
        Assert.False(disco.IsError, disco.Error);
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "resources:full"
            },
            cancel);
        Assert.False(tokenResponse.IsError, tokenResponse.Error);
        client.SetBearerToken(tokenResponse.AccessToken);
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
