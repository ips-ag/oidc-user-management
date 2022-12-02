using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.DbContexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUserModel, ApplicationRoleModel, string,
    IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim,
    IdentityUserToken<string>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUserModel>(
            user =>
            {
                user.Property(u => u.Id).ValueGeneratedOnAdd();
            });
        builder.Entity<ApplicationRoleModel>(
            role =>
            {
                role.HasKey(p => p.Id);
                role.Property(p => p.Id).ValueGeneratedOnAdd();
                role.Property(p => p.ConcurrencyStamp).ValueGeneratedOnAddOrUpdate();
                role.Property(p => p.Name).IsRequired();
                role.Property(p => p.NormalizedName).IsRequired();
                role.Property(p => p.Description).HasMaxLength(300);
            });
        builder.Entity<ApplicationRoleClaim>(
            roleClaim =>
            {
                roleClaim.Property(p => p.PermissionId).IsRequired().HasMaxLength(200);
                roleClaim.Property(p => p.Id).ValueGeneratedOnAdd();
                roleClaim.Property(p => p.ClaimType).HasMaxLength(256).IsRequired();
                roleClaim.HasIndex(p => new { p.RoleId, p.PermissionId }).IsUnique();
            });
        base.OnModelCreating(builder);
    }
}
