using Microsoft.Data.Sqlite;

namespace IPS.UserManagement.Tests.Fixtures.Persistence;

public class SqlServer : IAsyncDisposable
{
    private readonly SqliteConnection _identityServer;
    private readonly SqliteConnection _userManagement;

    private bool _disposed;
    public string IdentityServerConnectionString { get; }
    public string UserManagementConnectionString { get; }

    public SqlServer()
    {
        IdentityServerConnectionString = "Data Source=IdentityServer.db;Cache=Shared"; // filesystem
        IdentityServerConnectionString += ";Mode=Memory"; // memory
        UserManagementConnectionString = "Data Source=UserManagement.db;Cache=Shared"; // filesystem
        UserManagementConnectionString += ";Mode=Memory"; // memory
        _identityServer = new SqliteConnection(IdentityServerConnectionString);
        _identityServer.Open();
        _userManagement = new SqliteConnection(UserManagementConnectionString);
        _userManagement.Open();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;
        await _identityServer.CloseAsync();
        await _userManagement.CloseAsync();
    }
}
