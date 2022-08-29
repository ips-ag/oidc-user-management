using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IPS.UserManagement.Tests.Fixtures.Identity;

public class IdentityServerStartup
{
    private readonly IConfiguration _configuration;

    public IdentityServerStartup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        var assembly = GetType().Assembly.GetName().Name;
        var connectionString = _configuration.GetConnectionString("SqlServer");
        services.AddIdentityServer()
            .AddConfigurationStore(
                store =>
                {
                    store.ConfigureDbContext = builder => builder.UseSqlite(
                        connectionString,
                        sql => sql.MigrationsAssembly(assembly));
                })
            .AddOperationalStore(store =>
            {
                store.ConfigureDbContext = builder => builder.UseSqlite(
                    connectionString,
                    sql => sql.MigrationsAssembly(assembly));
            });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.InitializeDatabase();
        app.UseIdentityServer();
    }
}
