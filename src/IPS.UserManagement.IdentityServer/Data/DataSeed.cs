using Duende.IdentityServer.Models;

namespace IPS.UserManagement.IdentityServer.Data;

internal class DataSeed : IDataSeed
{
    public IEnumerable<Client> GetClients()
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
                    "users:read",
                    "users:full",
                    "permission-assignments:read",
                    "permission-assignments:full",
                    "role-assignments:read",
                    "role-assignments:full"
                }
            }
        };
    }

    public IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new IdentityResource[]
        {
            new IdentityResources.OpenId(), new IdentityResources.Profile(), new IdentityResources.Email(),
            new IdentityResources.Phone()
        };
    }

    public IEnumerable<ApiScope> GetScopes()
    {
        return new[]
        {
            new ApiScope("resources:read"),
            new ApiScope("resources:full"),
            new ApiScope("permissions:read"),
            new ApiScope("permissions:full"),
            new ApiScope("roles:read"),
            new ApiScope("roles:full"),
            new ApiScope("users:read"),
            new ApiScope("users:full"),
            new ApiScope("permission-assignments:read"),
            new ApiScope("permission-assignments:full"),
            new ApiScope("role-assignments:read"),
            new ApiScope("role-assignments:full")
        };
    }

    public IEnumerable<ApiResource> GetApis()
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
                    "users:read",
                    "users:full",
                    "permission-assignments:read",
                    "permission-assignments:full",
                    "role-assignments:read",
                    "role-assignments:full"
                }
            }
        };
    }
}
