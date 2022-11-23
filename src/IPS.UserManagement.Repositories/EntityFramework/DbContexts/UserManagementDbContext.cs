using IPS.UserManagement.Repositories.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace IPS.UserManagement.Repositories.EntityFramework.DbContexts;

public class UserManagementDbContext : DbContext
{
    public DbSet<Role> Roles { get; set; }

    public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
        : base(options)
    {
    }
}
