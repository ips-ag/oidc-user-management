using Microsoft.Extensions.Options;

namespace IPS.UserManagement.Extensions.Authentication
{
    public class ConfigureAuthorizationOptions : IConfigureOptions<AuthorizationOptions>
    {
        public void Configure(AuthorizationOptions options)
        {
            options.AddPolicy("resources:full", policy => policy.RequireClaim("scope", "resources:full"));
            options.AddPolicy(
                "resources:read",
                policy => policy.RequireClaim("scope", "resources:read", "resources:full"));
        }
    }
}
