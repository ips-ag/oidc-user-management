using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IPS.UserManagement.Extensions.Authentication;

internal class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IServer _server;

    public ConfigureJwtBearerOptions(IServer server)
    {
        _server = server;
    }
    
    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        var addresses = _server.Features.Get<IServerAddressesFeature>()?.Addresses;
        options.Authority = addresses?.FirstOrDefault(address => address.StartsWith("https://")) ??
            addresses?.FirstOrDefault();
        options.TokenValidationParameters = new TokenValidationParameters { ValidAudience = "usermanagement" };
        options.RequireHttpsMetadata = false;
        options.IncludeErrorDetails = true;
        options.Events = new JwtBearerEvents
        {
            OnChallenge = c =>
            {
                c.HandleResponse();
                if (!c.HttpContext.Response.HasStarted)
                {
                    c.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = c =>
            {
                c.NoResult();
                c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                c.Response.ContentType = "text/plain";
                return c.Response.WriteAsync(
                    $"An error occurred processing your authentication: {c.Exception.Message}");
            }
        };
    }
}
