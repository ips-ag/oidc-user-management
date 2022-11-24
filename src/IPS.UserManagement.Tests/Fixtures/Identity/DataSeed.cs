﻿using Duende.IdentityServer.Models;
using IPS.UserManagement.IdentityServer.Data;

namespace IPS.UserManagement.Tests.Fixtures.Identity;

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
                    "permission-assignments:read",
                    "permission-assignments:full",
                    "role-assignments:read",
                    "role-assignments:full"
                }
            },
            new Client
            {
                ClientId = "Bob",
                ClientName = "Temporary user via client credentials",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials
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
                    "permission-assignments:read",
                    "permission-assignments:full",
                    "role-assignments:read",
                    "role-assignments:full"
                }
            }
        };
    }
}
