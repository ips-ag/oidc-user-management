using IPS.UserManagement.IdentityServer.Extensions;

namespace IPS.UserManagement.IdentityServer;

public class Startup
{
    private static bool s_configured;
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        if (s_configured) return;
        services.ConfigureIdentityServer(Configuration);
        s_configured = true;
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseIdentityServer();
    }
}
