using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class CreateAdministratorRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ApplicationRoleModel administrator = new()
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "Administrator",
                NormalizedName = "Administrator",
                Description = "Allows complete control over User Management",
                Created = DateTime.UtcNow
            };
            migrationBuilder.InsertData(
                "AspNetRoles",
                new[]
                {
                    nameof(ApplicationRoleModel.Id), nameof(ApplicationRoleModel.Name),
                    nameof(ApplicationRoleModel.NormalizedName), nameof(ApplicationRoleModel.Description),
                    nameof(ApplicationRoleModel.Created)
                },
                new object[]
                {
                    administrator.Id, administrator.Name, administrator.NormalizedName, administrator.Description,
                    administrator.Created
                });
            migrationBuilder.InsertData(
                "AspNetRoleClaims",
                new[]
                {
                    nameof(ApplicationRoleClaim.ClaimType), nameof(ApplicationRoleClaim.ClaimValue),
                    nameof(ApplicationRoleClaim.RoleId)
                },
                new object[,]
                {
                    { "resources:full", "resources:full", administrator.Id },
                    { "permissions:full", "permissions:full", administrator.Id },
                    { "roles:full", "roles:full", administrator.Id },
                    { "users:full", "users:full", administrator.Id },
                    { "permission-assignments:full", "permission-assignments:full", administrator.Id },
                    { "role-assignments:full", "role-assignments:full", administrator.Id }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("AspNetRoles", "NormalizedName", "Administrator");
            migrationBuilder.DeleteData(
                "AspNetRoleClaims",
                "ClaimType",
                new[]
                {
                    "resources:full", "permissions:full", "roles:full", "users:full", "permission-assignments:full",
                    "role-assignments:full"
                });
        }
    }
}
