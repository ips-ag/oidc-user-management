using IPS.UserManagement.Tests.Fixtures.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IPS.UserManagement.Tests.Fixtures;

public class HostFixture : IDisposable
{
    private readonly Lazy<UserManagementApplicationFactory> _serverLazy;
    private readonly Lazy<IdentityServerFactory> _identityServerLazy;
    public IServiceProvider Services => _serverLazy.Value.Services;

    public HttpClient Client => _serverLazy.Value.CreateClient(
        new WebApplicationFactoryClientOptions { BaseAddress = new Uri("http://usermanagement") });

    public HttpClient IdentityServerClient => _identityServerLazy.Value.CreateClient(
        new WebApplicationFactoryClientOptions { BaseAddress = new Uri("http://localhost") });

    public ITestOutputHelper? TestOutputHelper { private get; set; }

    public HostFixture()
    {
        _serverLazy = new Lazy<UserManagementApplicationFactory>(
            () => new UserManagementApplicationFactory(() => TestOutputHelper, IdentityServerClient));
        _identityServerLazy = new Lazy<IdentityServerFactory>(() => new IdentityServerFactory(() => TestOutputHelper));
    }

    public void Dispose()
    {
        if (_identityServerLazy.IsValueCreated)
        {
            _identityServerLazy.Value.Dispose();
        }
        if (_serverLazy.IsValueCreated)
        {
            _serverLazy.Value.Dispose();
        }
    }
}
