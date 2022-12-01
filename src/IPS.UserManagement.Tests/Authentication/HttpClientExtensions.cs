using IdentityModel.Client;

namespace IPS.UserManagement.Tests.Authentication;

public static class HttpClientExtensions
{
    public static async Task LoginUserAsync(
        this HttpClient client,
        HttpClient identityServerClient,
        CancellationToken cancel,
        string userName,
        string password,
        string clientId,
        string scope)
    {
        var disco = await identityServerClient.GetDiscoveryDocumentAsync(cancellationToken: cancel);
        Assert.False(disco.IsError, disco.Error);
        var tokenResponse = await identityServerClient.RequestPasswordTokenAsync(
            new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                UserName = userName,
                Password = password,
                Scope = scope,
                ClientId = clientId
            },
            cancel);
        Assert.False(tokenResponse.IsError, tokenResponse.Error);
        client.SetBearerToken(tokenResponse.AccessToken);
    }
}
