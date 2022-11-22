using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using IPS.UserManagement.Application.Extensions;
using IPS.UserManagement.Extensions.Authentication;
using IPS.UserManagement.Extensions.Errors;
using IPS.UserManagement.IdentityServer.Extensions;
using IPS.UserManagement.Repositories.IdentityServer.Extensions;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace IPS.UserManagement;

public class Startup
{
    private IConfiguration Configuration { get; }
    private IHostEnvironment Env { get; }
    private bool UseIdentityServer => Configuration.GetValue("IdentityServer:Enabled", false);

    public Startup(IConfiguration configuration, IHostEnvironment env)
    {
        Configuration = configuration;
        Env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddProblemDetailsConventions()
            .AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(
                        new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
                });
        services.AddProblemDetailsServices();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services
            .AddAuthentication()
            .AddJwtBearer();
        services.AddSingleton<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticationOptions>();
        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
        services.AddAuthorization();
        services.AddSingleton<IConfigureOptions<AuthorizationOptions>, ConfigureAuthorizationOptions>();
        if (UseIdentityServer) services.ConfigureIdentityServer(Configuration);

        services
            .AddApplicationServices()
            .AddIdentityServerRepositories(Configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseProblemDetails();
        if (Env.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseRouting();
        if (UseIdentityServer)
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
