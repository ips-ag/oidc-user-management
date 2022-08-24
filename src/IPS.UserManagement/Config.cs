using Duende.IdentityServer.Models;

namespace IPS.UserManagement;

public static class Config
{
    public static IEnumerable<Client> GetClients()
    {
        return new[]
        {
            new Client
            {
                ClientId = "client",
                ClientName = "Client credentials flow client",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes =
                {
                    "resources:read",
                    "resources:full",
                    "permissions:read",
                    "permissions:full",
                    "roles:read",
                    "roles:full",
                    "permission-assignments:read",
                    "permission-assignments:full",
                    "role-assignments:read",
                    "role-assignments:full"
                }
            }
        };
    }

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new IdentityResource[]
        {
            new IdentityResources.OpenId(), new IdentityResources.Profile(), new IdentityResources.Email(),
            new IdentityResources.Phone()
        };
    }

    public static IEnumerable<ApiScope> GetScopes()
    {
        return new[]
        {
            new ApiScope("resources:read", new[] { "resources:read" }),
            new ApiScope("resources:full", new[] { "resources:read", "resources:full" }),
            new ApiScope("permissions:read", new[] { "permissions:read" }),
            new ApiScope("permissions:full", new[] { "permissions:read", "permissions:full" }),
            new ApiScope("roles:read", new[] { "roles:read" }),
            new ApiScope("roles:full", new[] { "roles:read", "roles:full" }),
            new ApiScope("permission-assignments:read", new[] { "permission-assignments:read" }), new ApiScope(
                "permission-assignments:full",
                new[] { "permission-assignments:read", "permission-assignments:full" }),
            new ApiScope("role-assignments:read", new[] { "role-assignments:read" }),
            new ApiScope("role-assignments:full", new[] { "role-assignments:read", "role-assignments:full" })
        };
    }

    public static IEnumerable<ApiResource> GetApis()
    {
        return new[]
        {
            new ApiResource("usermanagement", "User Management")
            {
                Scopes = new List<string>
                {
                    "resources:read",
                    "resources:full",
                    "permissions:read",
                    "permissions:full",
                    "roles:read",
                    "roles:full",
                    "permission-assignments:read",
                    "permission-assignments:full",
                    "role-assignments:read",
                    "role-assignments:full"
                }
            }
        };
    }
}
