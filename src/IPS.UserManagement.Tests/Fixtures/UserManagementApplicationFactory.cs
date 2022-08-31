using IPS.UserManagement.Tests.Fixtures.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IPS.UserManagement.Tests.Fixtures;

public class UserManagementApplicationFactory : WebApplicationFactory<Program>
{
    private readonly HttpClient _identityServerClient;
    private readonly string _connectionString;
    private readonly Func<ITestOutputHelper?> _testOutputHelper;

    public UserManagementApplicationFactory(
        Func<ITestOutputHelper?> testOutputHelper,
        HttpClient identityServerClient,
        string connectionString)
    {
        _identityServerClient = identityServerClient;
        _connectionString = connectionString;
        _testOutputHelper = testOutputHelper;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(
            (context, config) =>
            {
                config.Sources.Clear();
                config.AddJsonFile(
                        $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                        false,
                        true)
                    .AddInMemoryCollection(
                        new Dictionary<string, string> { ["ConnectionStrings:SqlServer"] = _connectionString })
                    .AddEnvironmentVariables();
            });
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureLogging(
                (_, logging) =>
                {
                    var testOutputHelper = _testOutputHelper();
                    if (testOutputHelper is null) return;
                    logging.AddXunit(testOutputHelper, LogLevel.Warning);
                })
            .UseUrls("http://usermanagement")
            .UseEnvironment("Test")
            .UseContentRoot(Path.GetDirectoryName(GetType().Assembly.Location))
            .ConfigureTestServices(
                services =>
                {
                    services.AddSingleton(_ => _identityServerClient);
                    services.AddSingleton<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticationOptions>();
                    services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
                });
        base.ConfigureWebHost(builder);
    }
}
