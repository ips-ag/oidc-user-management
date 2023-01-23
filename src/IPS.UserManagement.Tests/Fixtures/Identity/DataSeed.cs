using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.Hosting;

namespace IPS.UserManagement.Tests.Fixtures.Identity;

internal class DataSeed : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DataSeed(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    private IEnumerable<Client> GetClients()
    {
        return new[]
        {
            new Client
            {
                ClientId = "usermanagement",
                ClientName = "UserManagement application",
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
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
            },
            new Client
            {
                ClientId = "erp",
                ClientName = "ERP application",
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = { "orders:read" }
            }
        };
    }

    private IEnumerable<IdentityResource> GetIdentityResources()
    {
        return Enumerable.Empty<IdentityResource>();
    }

    private IEnumerable<ApiScope> GetScopes()
    {
        return Enumerable.Empty<ApiScope>();
    }

    private IEnumerable<ApiResource> GetApis()
    {
        return Enumerable.Empty<ApiResource>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var services = scope.ServiceProvider;
        var configurationContext = services.GetRequiredService<ConfigurationDbContext>();
        foreach (var client in GetClients())
        {
            configurationContext.Clients.Add(client.ToEntity());
        }
        foreach (var resource in GetIdentityResources())
        {
            configurationContext.IdentityResources.Add(resource.ToEntity());
        }
        foreach (var resource in GetScopes())
        {
            configurationContext.ApiScopes.Add(resource.ToEntity());
        }
        foreach (var resource in GetApis())
        {
            configurationContext.ApiResources.Add(resource.ToEntity());
        }
        await configurationContext.SaveChangesAsync(stoppingToken);
    }
}
