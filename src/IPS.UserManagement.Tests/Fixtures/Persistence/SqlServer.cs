using Microsoft.Data.Sqlite;

namespace IPS.UserManagement.Tests.Fixtures.Persistence;

public class SqlServer : IAsyncDisposable
{
    private readonly SqliteConnection _identityServer;
    private readonly SqliteConnection _userManagement;
    private readonly SqliteConnection _aspNetCoreIdentity;

    private bool _disposed;
    public string IdentityServerConnectionString { get; }
    public string UserManagementConnectionString { get; }
    public string AspNetCoreIdentityConnectionString { get; }

    public SqlServer()
    {
        IdentityServerConnectionString = "Data Source=IdentityServer.db;Cache=Shared"; // filesystem
        IdentityServerConnectionString += ";Mode=Memory"; // memory
        UserManagementConnectionString = "Data Source=UserManagement.db;Cache=Shared"; // filesystem
        UserManagementConnectionString += ";Mode=Memory"; // memory
        AspNetCoreIdentityConnectionString = "Data Source=AspNetCoreIdentity.db;Cache=Shared"; // filesystem
        AspNetCoreIdentityConnectionString += ";Mode=Memory"; // memory
        _identityServer = new SqliteConnection(IdentityServerConnectionString);
        _identityServer.Open();
        _userManagement = new SqliteConnection(UserManagementConnectionString);
        _userManagement.Open();
        _aspNetCoreIdentity = new SqliteConnection(UserManagementConnectionString);
        _aspNetCoreIdentity.Open();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;
        await _identityServer.CloseAsync();
        await _userManagement.CloseAsync();
        await _aspNetCoreIdentity.CloseAsync();
    }
}
