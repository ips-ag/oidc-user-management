﻿using IPS.UserManagement.Repositories.EntityFramework;
using IPS.UserManagement.Repositories.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IPS.UserManagement.Repositories.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UserManagement") ??
            throw new InvalidOperationException("UserManagement ConnectionString not configured");
        services.AddDbContext<UserManagementDbContext>(options => options.UseSqlite(connectionString));
        services.AddHostedService<MigrationExecutor>();
        return services;
    }
}
