using IPS.UserManagement.Tests.Fixtures.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IPS.UserManagement.Tests.Fixtures;

public class HostFixture : IDisposable
{
    private readonly Lazy<WebApplicationFactory<Program>> _serverLazy;
    public IServiceProvider Services => _serverLazy.Value.Services;
    public HttpClient Client => _serverLazy.Value.CreateClient();
    public ITestOutputHelper? TestOutputHelper { private get; set; }

    public HostFixture()
    {
        _serverLazy = new Lazy<WebApplicationFactory<Program>>(
            () =>
            {
                return new WebApplicationFactory<Program>().WithWebHostBuilder(
                    webHost => webHost
                        .ConfigureLogging(
                            (_, logging) =>
                            {
                                if (TestOutputHelper is null) return;
                                logging.AddXunit(TestOutputHelper, LogLevel.Warning);
                            })
                        .UseEnvironment("Test")
                        .UseContentRoot(Path.GetDirectoryName(GetType().Assembly.Location))
                        .ConfigureAppConfiguration(
                            (context, config) =>
                            {
                                config.Sources.Clear();
                                config.AddJsonFile(
                                        $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                                        false,
                                        true)
                                    .AddEnvironmentVariables();
                            })
                        .ConfigureTestServices((services)=>
                        {
                            services.AddSingleton(_ => Client);
                            AddTestServices(services);
                        })
                );
            });
    }

    public void Dispose()
    {
        if (!_serverLazy.IsValueCreated) return;
        Client.Dispose();
        _serverLazy.Value.Dispose();
    }

    private void AddTestServices(IServiceCollection services)
    {
        services.AddSingleton<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticationOptions>();
        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
    }
}
