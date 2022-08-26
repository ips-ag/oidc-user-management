using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IPS.UserManagement.Tests.Fixtures.Identity;

public class IdentityServerFactory : WebApplicationFactory<IdentityServerStartup>
{
    private readonly Func<ITestOutputHelper?> _testOutputHelper;

    public IdentityServerFactory(Func<ITestOutputHelper?> testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    protected override IWebHostBuilder CreateWebHostBuilder()
    {
        return new WebHostBuilder().UseTestServer();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseStartup<IdentityServerStartup>()
            .ConfigureLogging(
                (_, logging) =>
                {
                    var output = _testOutputHelper();
                    if (output is null) return;
                    logging.AddXunit(output, LogLevel.Warning);
                })
            .UseUrls("http://localhost")
            .UseEnvironment("Test")
            .ConfigureAppConfiguration(
                (context, config) =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile(
                            $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                            true,
                            true)
                        .AddEnvironmentVariables();
                });
        base.ConfigureWebHost(builder);
    }
}
