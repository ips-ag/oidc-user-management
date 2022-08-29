using IPS.UserManagement;
using IPS.UserManagement.Application.Extensions;
using IPS.UserManagement.Extensions.Authentication;
using IPS.UserManagement.Repositories.Memory.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(
        (ctx, lc) => lc
            .WriteTo.Console()
            .ReadFrom.Configuration(ctx.Configuration));
    builder.Services
        .AddControllers()
        .AddNewtonsoftJson(
            options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(
                    new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
            });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    var identityServerEnabled = builder.Configuration.GetValue<bool>("IdentityServer:Enabled");
    if (identityServerEnabled)
    {
        builder.Services.AddIdentityServer()
            .AddInMemoryClients(Config.GetClients())
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryApiScopes(Config.GetScopes())
            .AddInMemoryApiResources(Config.GetApis());
    }
    builder.Services
        .AddAuthentication()
        .AddJwtBearer();
    builder.Services.AddSingleton<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticationOptions>();
    builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
    builder.Services.AddAuthorization();
    builder.Services.AddSingleton<IConfigureOptions<AuthorizationOptions>, ConfigureAuthorizationOptions>();

    builder.Services
        .AddApplicationServices()
        .AddRepositories();

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseRouting();
    if (identityServerEnabled)
    {
        app.UseIdentityServer();
    }
    else
    {
        app.UseAuthentication();
    }
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Error running application");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{
}
