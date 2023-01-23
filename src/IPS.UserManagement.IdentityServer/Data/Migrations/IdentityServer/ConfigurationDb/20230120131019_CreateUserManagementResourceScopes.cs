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
                { "users:read", userManagement.Id }, { "users:full", userManagement.Id },
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

        migrationBuilder.InsertData(
            "IdentityResources",
            new[]
            {
                nameof(IdentityResource.Id), nameof(IdentityResource.Enabled), nameof(IdentityResource.Name),
                nameof(IdentityResource.DisplayName), nameof(IdentityResource.Description),
                nameof(IdentityResource.Required), nameof(IdentityResource.Emphasize),
                nameof(IdentityResource.ShowInDiscoveryDocument), nameof(IdentityResource.Created),
                nameof(IdentityResource.NonEditable)
            },
            new object[,]
            {
                { 1, true, "openid", "Your user identifier", null, true, false, true, DateTime.UtcNow, false },
                {
                    2, true, "profile", "User profile",
                    "Your user profile information (first name, last name, etc.)", false, true, true,
                    DateTime.UtcNow, false
                },
                { 3, true, "email", "Your email address", null, false, true, true, DateTime.UtcNow, false },
                { 4, true, "phone", "Your phone number", null, false, true, true, DateTime.UtcNow, false }
            });

        migrationBuilder.InsertData(
            "IdentityResourceClaims",
            new[] { nameof(IdentityResourceClaim.IdentityResourceId), nameof(IdentityResourceClaim.Type) },
            new object[,]
            {
                { 1, "sub" }, { 2, "name" }, { 2, "family_name" }, { 2, "given_name" }, { 2, "middle_name" },
                { 2, "nickname" }, { 2, "preferred_username" }, { 2, "profile" }, { 2, "picture" },
                { 2, "website" }, { 2, "gender" }, { 2, "birthdate" }, { 2, "zoneinfo" }, { 2, "locale" },
                { 2, "updated_at" }, { 3, "email" }, { 3, "email_verified" }, { 4, "phone_number" },
                { 4, "phone_number_verified" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData("ApiResources", nameof(ApiResource.Name), "usermanagement");
        migrationBuilder.DeleteData(
            "ApiScopes",
            nameof(ApiScope.Name),
            new object[]
            {
                "resources:read", "resources:read", "permissions:read", "permissions:read", "roles:read",
                "roles:read", "users:read", "users:read", "permission-assignments:read",
                "permission-assignments:read", "role-assignments:read", "role-assignments:read"
            });
        migrationBuilder.DeleteData(
            "IdentityResources",
            nameof(IdentityResource.Name),
            new object[] { "openid", "profile", "email", "phone" });
    }
}
