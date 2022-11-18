using IdentityModel.Client;

namespace IPS.UserManagement.Tests.Authentication;

public static class HttpClientExtensions
{
    public static async Task LoginAsync(
        this HttpClient client,
        HttpClient identityServerClient,
        CancellationToken cancel,
        string clientId = "client",
        string clientSecret = "secret",
        string scope = "")
    {
        var disco = await identityServerClient.GetDiscoveryDocumentAsync(cancellationToken: cancel);
        Assert.False(disco.IsError, disco.Error);
        var tokenResponse = await identityServerClient.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint, ClientId = clientId, ClientSecret = clientSecret, Scope = scope
            },
            cancel);
        Assert.False(tokenResponse.IsError, tokenResponse.Error);
        client.SetBearerToken(tokenResponse.AccessToken);
    }
}
