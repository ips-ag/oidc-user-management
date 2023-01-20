using IPS.UserManagement.Tests.Fixtures.Identity;
using IPS.UserManagement.Tests.Fixtures.Persistence;
using IPS.UserManagement.Tests.Fixtures.Resources;
using IPS.UserManagement.Tests.Fixtures.UserManagement;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IPS.UserManagement.Tests.Fixtures;

public sealed class HostFixture : IAsyncDisposable
{
    private readonly Lazy<ErpSystemApplicationFactory> _erpLazy;
    private readonly Lazy<UserManagementApplicationFactory> _userManagementLazy;
    private readonly Lazy<IdentityServerFactory> _identityServerLazy;
    private readonly Lazy<SqlServer> _sqlServerLazy;

    public HttpClient UserManagementClient => _userManagementLazy.Value.CreateClient(
        new WebApplicationFactoryClientOptions { BaseAddress = new Uri("http://usermanagement") });

    public HttpClient ErpClient => _erpLazy.Value.CreateClient(
        new WebApplicationFactoryClientOptions { BaseAddress = new Uri("http://erp") });

    public HttpClient IdentityServerClient => _identityServerLazy.Value.CreateClient(
        new WebApplicationFactoryClientOptions { BaseAddress = new Uri("http://localhost") });

    public ITestOutputHelper? TestOutputHelper { private get; set; }
    private string IdentityServerConnectionString => _sqlServerLazy.Value.IdentityServerConnectionString;
    private string AspNetCoreIdentityConnectionString => _sqlServerLazy.Value.AspNetCoreIdentityConnectionString;

    public HostFixture()
    {
        _sqlServerLazy = new Lazy<SqlServer>(() => new SqlServer());
        _identityServerLazy =
            new Lazy<IdentityServerFactory>(
                () => new IdentityServerFactory(
                    () => TestOutputHelper,
                    IdentityServerConnectionString,
                    AspNetCoreIdentityConnectionString));
        _userManagementLazy = new Lazy<UserManagementApplicationFactory>(
            () => new UserManagementApplicationFactory(
                () => TestOutputHelper,
                IdentityServerClient,
                IdentityServerConnectionString,
                AspNetCoreIdentityConnectionString));
        _erpLazy = new Lazy<ErpSystemApplicationFactory>(
            () => new ErpSystemApplicationFactory(
                () => TestOutputHelper,
                IdentityServerClient));
    }

    public async ValueTask DisposeAsync()
    {
        if (_identityServerLazy.IsValueCreated)
        {
            await _identityServerLazy.Value.DisposeAsync();
        }
        if (_userManagementLazy.IsValueCreated)
        {
            await _userManagementLazy.Value.DisposeAsync();
        }
        if (_sqlServerLazy.IsValueCreated)
        {
            await _sqlServerLazy.Value.DisposeAsync();
        }
    }
}
