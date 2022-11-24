using IPS.UserManagement.Repositories.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace IPS.UserManagement.Repositories.EntityFramework.DbContexts;

public class UserManagementDbContext : DbContext
{
    public DbSet<RoleModel> Roles { get; set; }

    public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoleModel>(
            role =>
            {
                // key
                role.HasKey(x => x.Id);

                // properties
                role.Property(x => x.Name).HasMaxLength(100).IsRequired();
                role.Property(x => x.Description).HasMaxLength(300);

                // indexes
                role.HasIndex(x => x.Name).IsUnique();

                // foreign keys
                role.HasMany(x => x.Permissions)
                    .WithOne(x => x.Role)
                    .HasForeignKey(x => x.RoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

        modelBuilder.Entity<RolePermissionModel>(
            permission =>
            {
                permission.HasKey(x => x.Id);
                permission.Property(x => x.PermissionId).HasMaxLength(200).IsRequired();
                permission.HasIndex(x => new { x.RoleId, Permission = x.PermissionId }).IsUnique();
            });

        base.OnModelCreating(modelBuilder);
    }
}
