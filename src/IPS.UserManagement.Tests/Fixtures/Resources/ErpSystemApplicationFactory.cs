using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace IPS.UserManagement.Tests.Fixtures.Resources;

public class ErpSystemApplicationFactory : WebApplicationFactory<ErpSystemApplicationFactory>
{
    private readonly HttpClient _identityServerClient;
    private readonly Func<ITestOutputHelper?> _testOutputHelper;

    public ErpSystemApplicationFactory(
        Func<ITestOutputHelper?> testOutputHelper,
        HttpClient identityServerClient)
    {
        _identityServerClient = identityServerClient;
        _testOutputHelper = testOutputHelper;
    }

    private void ConfigureLogging(HostBuilderContext ctx, LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.TestOutput(_testOutputHelper(), LogEventLevel.Warning);
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
            .UseUrls("http://erp")
            .UseEnvironment("Test")
            .UseContentRoot(contentRoot)
            .ConfigureServices(
                services =>
                {
                    services.AddControllers();
                    services.AddAuthentication(
                        options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        }).AddJwtBearer(
                        options =>
                        {
                            options.Backchannel = _identityServerClient;
                            options.Authority = _identityServerClient.BaseAddress?.OriginalString;
                            options.TokenValidationParameters = new TokenValidationParameters { ValidAudience = "ERP" };
                            options.RequireHttpsMetadata = false;
                            options.IncludeErrorDetails = true;
                        });
                    services.AddAuthorization(
                        options =>
                        {
                            options.AddPolicy("orders:full", policy => policy.RequireClaim("scope", "orders:full"));
                            options.AddPolicy(
                                "orders:read",
                                policy => policy.RequireClaim("scope", "orders:read", "orders:full"));
                        });
                })
            .Configure(
                app =>
                {
                    app.UseRouting();
                    app.UseAuthentication();
                    app.UseAuthorization();
                    app.UseEndpoints(
                        endpoints =>
                        {
                            endpoints.MapGet(
                                "/orders",
                                [Authorize(Policy = "orders:read")]() => Results.Ok());
                        });
                });
        base.ConfigureWebHost(builder);
    }
}
