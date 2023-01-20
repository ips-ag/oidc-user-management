#nullable disable

using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IPS.UserManagement.IdentityServer.Data.Migrations.IdentityServer.ConfigurationDb;

/// <inheritdoc />
public partial class CreateUserManagementResourceScopes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        ApiResource userManagement = new()
        {
            Id = 1,
            Enabled = true,
            Name = "usermanagement",
            DisplayName = "User Management",
            ShowInDiscoveryDocument = true,
            RequireResourceIndicator = false,
            Created = DateTime.UtcNow,
            NonEditable = false
        };
        migrationBuilder.InsertData(
            "ApiResources",
            new[]
            {
                nameof(ApiResource.Id), nameof(ApiResource.Enabled), nameof(ApiResource.Name),
                nameof(ApiResource.DisplayName), nameof(ApiResource.ShowInDiscoveryDocument),
                nameof(ApiResource.RequireResourceIndicator), nameof(ApiResource.Created),
                nameof(ApiResource.NonEditable)
            },
            new object[]
            {
                userManagement.Id, userManagement.Enabled, userManagement.Name, userManagement.DisplayName,
                userManagement.ShowInDiscoveryDocument, userManagement.RequireResourceIndicator,
                userManagement.Created, userManagement.NonEditable
            });

        migrationBuilder.InsertData(
            "ApiResourceScopes",
            new[] { nameof(ApiResourceScope.Scope), nameof(ApiResourceScope.ApiResourceId) },
            new object[,]
            {
                { "resources:read", userManagement.Id }, { "resources:full", userManagement.Id },
                { "permissions:read", userManagement.Id }, { "permissions:full", userManagement.Id },
                { "roles:read", userManagement.Id }, { "roles:full", userManagement.Id },
                { "permission-assignments:read", userManagement.Id },
                { "permission-assignments:full", userManagement.Id },
                { "role-assignments:read", userManagement.Id }, { "role-assignments:full", userManagement.Id }
            });

        migrationBuilder.InsertData(
            "ApiScopes",
            new[]
            {
                nameof(ApiScope.Enabled), nameof(ApiScope.Name), nameof(ApiScope.DisplayName),
                nameof(ApiScope.Required), nameof(ApiScope.Emphasize), nameof(ApiScope.ShowInDiscoveryDocument),
                nameof(ApiScope.Created), nameof(ApiScope.NonEditable)
            },
            new object[,]
            {
                { true, "resources:read", "resources:read", false, false, true, DateTime.UtcNow, false },
                { true, "resources:full", "resources:full", false, false, true, DateTime.UtcNow, false },
                { true, "permissions:read", "permissions:read", false, false, true, DateTime.UtcNow, false },
                { true, "permissions:full", "permissions:full", false, false, true, DateTime.UtcNow, false },
                { true, "roles:read", "roles:read", false, false, true, DateTime.UtcNow, false },
                { true, "roles:full", "roles:full", false, false, true, DateTime.UtcNow, false },
                { true, "users:read", "users:read", false, false, true, DateTime.UtcNow, false },
                { true, "users:full", "users:full", false, false, true, DateTime.UtcNow, false },
                {
                    true, "permission-assignments:read", "permission-assignments:read", false, false, true,
                    DateTime.UtcNow, false
                },
                {
                    true, "permission-assignments:full", "permission-assignments:full", false, false, true,
                    DateTime.UtcNow, false
                },
                {
                    true, "role-assignments:read", "role-assignments:read", false, false, true, DateTime.UtcNow,
                    false
                },
                {
                    true, "role-assignments:full", "role-assignments:full", false, false, true, DateTime.UtcNow,
                    false
                }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
