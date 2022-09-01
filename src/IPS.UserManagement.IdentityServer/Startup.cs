using IPS.UserManagement.IdentityServer.Extensions;

namespace IPS.UserManagement.IdentityServer;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureIdentityServer(Configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseIdentityServer();
    }
}
