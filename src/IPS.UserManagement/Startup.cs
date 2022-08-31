using IPS.UserManagement.Application.Extensions;
using IPS.UserManagement.Extensions.Authentication;
using IPS.UserManagement.Repositories.IdentityServer.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace IPS.UserManagement;

public class Startup
{
    private IConfiguration Configuration { get; }
    private IHostEnvironment Env { get; }

    public Startup(IConfiguration configuration, IHostEnvironment env)
    {
        Configuration = configuration;
        Env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(
                        new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
                });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services
            .AddAuthentication()
            .AddJwtBearer();
        services.AddSingleton<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticationOptions>();
        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
        services.AddAuthorization();
        services.AddSingleton<IConfigureOptions<AuthorizationOptions>, ConfigureAuthorizationOptions>();

        services
            .AddApplicationServices()
            .AddIdentityServerRepositories(Configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        if (Env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseRouting();
        var identityServerEnabled = Configuration.GetValue<bool>("IdentityServer:Enabled");
        if (identityServerEnabled)
        {
            app.UseIdentityServer();
        }
        else
        {
            app.UseAuthentication();
        }
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
