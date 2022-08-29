using Microsoft.Data.Sqlite;

namespace IPS.UserManagement.Tests.Fixtures.Persistence;

public class SqlServer : IAsyncDisposable
{
    private readonly SqliteConnection _connection;

    private bool _disposed;
    public string ConnectionString { get; }

    public SqlServer()
    {
        ConnectionString = "Data Source=IdentityServer.db;Cache=Shared"; // filesystem
        ConnectionString += ";Mode=Memory"; // memory
        _connection = new SqliteConnection(ConnectionString);
        _connection.Open();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;
        await _connection.CloseAsync();
    }
}
