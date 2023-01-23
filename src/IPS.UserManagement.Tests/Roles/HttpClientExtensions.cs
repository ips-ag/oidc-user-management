using System.Net.Http.Json;
using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Application.Features.Roles.Models;
using Microsoft.AspNetCore.Mvc;

namespace IPS.UserManagement.Tests.Roles;

public static class HttpClientExtensions
{
    public static async Task<RoleQueryModel> CreateRoleAsync(
        this HttpClient client,
        CreateRoleCommandModel command,
        CancellationToken cancel)
    {
        var content = JsonContent.Create(command);
        var response = await client.PostAsync("roles", content, cancel);
        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancel);
            Assert.NotNull(problem);
        }
        response.EnsureSuccessStatusCode();
        var model = await response.Content.ReadFromJsonAsync<RoleQueryModel>(cancellationToken: cancel);
        Assert.NotNull(model);
        return model;
    }

    public static async Task<List<RoleQueryModel>> GetRolesAsync(
        this HttpClient client,
        CancellationToken cancel)
    {
        var response = await client.GetAsync("roles", cancel);
        response.EnsureSuccessStatusCode();
        var models = await response.Content.ReadFromJsonAsync<List<RoleQueryModel>>(cancellationToken: cancel);
        Assert.NotNull(models);
        return models;
    }

    public static async Task<PermissionQueryModel> AssignPermissionAsync(
        this HttpClient client,
        string roleId,
        CreatePermissionAssignmentCommandModel command,
        CancellationToken cancel)
    {
        var content = JsonContent.Create(command);
        var response = await client.PostAsync($"roles/{roleId}/permissions", content, cancel);
        response.EnsureSuccessStatusCode();
        var model = await response.Content.ReadFromJsonAsync<PermissionQueryModel>(cancellationToken: cancel);
        Assert.NotNull(model);
        return model;
    }

    public static async Task<List<PermissionQueryModel>> GetPermissionsForRoleAsync(
        this HttpClient client,
        string roleId,
        CancellationToken cancel)
    {
        var response = await client.GetAsync($"roles/{roleId}/permissions", cancel);
        response.EnsureSuccessStatusCode();
        var models = await response.Content.ReadFromJsonAsync<List<PermissionQueryModel>>(cancellationToken: cancel);
        Assert.NotNull(models);
        return models;
    }
}
