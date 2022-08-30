using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace IPS.UserManagement.Tests.Fixtures.Identity;

internal static class HostingExtensions
{
    public static void InitializeDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var persistedGrandContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
        persistedGrandContext.Database.Migrate();
        var configurationContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        configurationContext.Database.Migrate();
        if (!configurationContext.Clients.Any())
        {
            foreach (var client in Config.GetClients())
            {
                configurationContext.Clients.Add(client.ToEntity());
            }
            configurationContext.SaveChanges();
        }
        if (!configurationContext.IdentityResources.Any())
        {
            foreach (var resource in Config.GetIdentityResources())
            {
                configurationContext.IdentityResources.Add(resource.ToEntity());
            }
            configurationContext.SaveChanges();
        }
        if (!configurationContext.ApiScopes.Any())
        {
            foreach (var resource in Config.GetScopes())
            {
                configurationContext.ApiScopes.Add(resource.ToEntity());
            }
            configurationContext.SaveChanges();
        }
        if (!configurationContext.ApiResources.Any())
        {
            foreach (var resource in Config.GetApis())
            {
                configurationContext.ApiResources.Add(resource.ToEntity());
            }
            configurationContext.SaveChanges();
        }
    }
}
