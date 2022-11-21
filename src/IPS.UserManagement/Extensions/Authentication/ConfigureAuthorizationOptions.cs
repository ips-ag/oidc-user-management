namespace IPS.UserManagement.Extensions.Authentication;

public class ConfigureAuthorizationOptions : IConfigureOptions<AuthorizationOptions>
{
    public void Configure(AuthorizationOptions options)
    {
        // resources
        options.AddPolicy("resources:full", policy => policy.RequireClaim("scope", "resources:full"));
        options.AddPolicy(
            "resources:read",
            policy => policy.RequireClaim("scope", "resources:read", "resources:full"));
        // permissions
        options.AddPolicy("permissions:full", policy => policy.RequireClaim("scope", "permissions:full"));
        options.AddPolicy(
            "permissions:read",
            policy => policy.RequireClaim("scope", "permissions:read", "permissions:full"));
    }
}
