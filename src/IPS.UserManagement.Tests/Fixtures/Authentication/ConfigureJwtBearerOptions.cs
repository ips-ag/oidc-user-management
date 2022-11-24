using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IPS.UserManagement.Tests.Fixtures.Authentication;

internal class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly HttpClient _client;

    public ConfigureJwtBearerOptions(HttpClient client)
    {
        _client = client;
    }
    
    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        options.Backchannel = _client;
        options.Authority = _client.BaseAddress?.OriginalString;
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
