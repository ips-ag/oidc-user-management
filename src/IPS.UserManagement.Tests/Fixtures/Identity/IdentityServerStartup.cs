using Microsoft.AspNetCore.Builder;

namespace IPS.UserManagement.Tests.Fixtures.Identity;

public class IdentityServerStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentityServer()
            .AddInMemoryClients(Config.GetClients())
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryApiScopes(Config.GetScopes())
            .AddInMemoryApiResources(Config.GetApis());
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseIdentityServer();
    }
}
