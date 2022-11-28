using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.DbContexts;
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
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddHostedService<MigrationExecutor>();
        return services;
    }
}
