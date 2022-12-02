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
        // roles
        options.AddPolicy("roles:full", policy => policy.RequireClaim("scope", "roles:full"));
        options.AddPolicy(
            "roles:read",
            policy => policy.RequireClaim("scope", "roles:read", "roles:full"));
        // permission role assignments
        options.AddPolicy(
            "permission-assignments:full",
            policy => policy.RequireClaim("scope", "permission-assignments:full"));
        options.AddPolicy(
            "permission-assignments:read",
            policy => policy.RequireClaim("scope", "permission-assignments:read", "permission-assignments:full"));
        // users
        options.AddPolicy(
            "users:full",
            policy => policy.RequireClaim("scope", "users:full"));
        options.AddPolicy(
            "users:read",
            policy => policy.RequireClaim("scope", "users:read", "users:full"));
        // role assignments
        options.AddPolicy(
            "role-assignments:full",
            policy => policy.RequireClaim("scope", "role-assignments:full"));
        options.AddPolicy(
            "role-assignments:read",
            policy => policy.RequireClaim("scope", "role-assignments:read", "role-assignments:full"));
    }
}
