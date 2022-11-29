using IPS.UserManagement.Domain.Roles;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.DbContexts;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.Roles;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.Roles.Converters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.Extensions;

public static class AspNetCoreIdentityRepositoryExtensions
{
    public static IServiceCollection AddAspNetCoreIdentityRepositories(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // entity framework
        var connectionString = configuration.GetConnectionString("AspNetCoreIdentity") ??
            throw new InvalidOperationException("AspNetCoreIdentity ConnectionString not configured");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
        services.AddSingleton<IConfigureOptions<IdentityOptions>, ConfigureIdentityOptions>();
        services.AddIdentity<ApplicationUserModel, ApplicationRoleModel>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddHostedService<MigrationExecutor>();
        // roles
        services.AddSingleton<ILookupNormalizer, LowerInvariantNormalizer>();
        services.AddSingleton<RoleConverter>();
        services.AddSingleton<PermissionConverter>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        return services;
    }
}
