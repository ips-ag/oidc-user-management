using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace IPS.UserManagement.Tests.Fixtures.Identity;

public class IdentityServerFactory : WebApplicationFactory<IdentityServer.Startup>
{
    private readonly Func<ITestOutputHelper?> _testOutputHelper;
    private readonly string _connectionString;

    public IdentityServerFactory(Func<ITestOutputHelper?> testOutputHelper, string connectionString)
    {
        _testOutputHelper = testOutputHelper;
        _connectionString = connectionString;
    }

    private void ConfigureLogging(HostBuilderContext ctx, LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.TestOutput(_testOutputHelper(), restrictedToMinimumLevel: LogEventLevel.Error);
    }

    protected override IHostBuilder? CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder().UseSerilog(ConfigureLogging);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseStartup<IdentityServer.Startup>()
            .UseUrls("http://localhost")
            .UseEnvironment("Test")
            .ConfigureAppConfiguration(
                (context, config) =>
                {
                    config.Sources.Clear();
                    config
                        .AddJsonFile(
                            $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                            true,
                            true)
                        .AddInMemoryCollection(
                            new Dictionary<string, string> { ["ConnectionStrings:SqlServer"] = _connectionString })
                        .AddEnvironmentVariables();
                })
            .UseTestServer();
        base.ConfigureWebHost(builder);
    }
}
