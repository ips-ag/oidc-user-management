using IPS.UserManagement.Tests.Fixtures.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace IPS.UserManagement.Tests.Fixtures;

public class UserManagementApplicationFactory : WebApplicationFactory<Startup>
{
    private readonly HttpClient _identityServerClient;
    private readonly string _identityServerConnectionString;
    private readonly string _userManagementConnectionString;
    private readonly string _aspNetCoreIdentityConnectionString;
    private readonly Func<ITestOutputHelper?> _testOutputHelper;

    public UserManagementApplicationFactory(
        Func<ITestOutputHelper?> testOutputHelper,
        HttpClient identityServerClient,
        string identityServerConnectionString,
        string userManagementConnectionString,
        string aspNetCoreIdentityConnectionString)
    {
        _identityServerClient = identityServerClient;
        _identityServerConnectionString = identityServerConnectionString;
        _userManagementConnectionString = userManagementConnectionString;
        _aspNetCoreIdentityConnectionString = aspNetCoreIdentityConnectionString;
        _testOutputHelper = testOutputHelper;
    }

    private void ConfigureLogging(HostBuilderContext ctx, LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.TestOutput(_testOutputHelper(), restrictedToMinimumLevel: LogEventLevel.Warning);
    }

    protected override IHostBuilder? CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder().UseSerilog(ConfigureLogging);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var contentRoot = Path.GetDirectoryName(GetType().Assembly.Location) ??
            throw new InvalidOperationException("Unable to find content root");
        builder
            .UseStartup<Startup>()
            .UseUrls("http://usermanagement")
            .UseEnvironment("Test")
            .UseContentRoot(contentRoot)
            .ConfigureAppConfiguration(
                (context, config) =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile(
                            $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                            false,
                            true)
                        .AddInMemoryCollection(
                            new Dictionary<string, string?>
                            {
                                ["ConnectionStrings:IdentityServer"] = _identityServerConnectionString,
                                ["ConnectionStrings:UserManagement"] = _userManagementConnectionString,
                                ["ConnectionStrings:AspNetCoreIdentity"] = _aspNetCoreIdentityConnectionString
                            })
                        .AddEnvironmentVariables();
                })
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
