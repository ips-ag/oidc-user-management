using System.Net.Http.Json;
using IPS.UserManagement.Application.Features.Roles.Models;
using IPS.UserManagement.Application.Features.Users.Models;

namespace IPS.UserManagement.Tests.Users;

public static class HttpClientExtensions
{
    public static async Task<List<UserQueryModel>> GetUsersAsync(
        this HttpClient client,
        string? id,
        string? userName,
        CancellationToken cancel)
    {
        var response = await client.GetAsync($"users?id={id}&username={userName}", cancel);
        response.EnsureSuccessStatusCode();
        var models = await response.Content.ReadFromJsonAsync<List<UserQueryModel>>(cancellationToken: cancel);
        Assert.NotNull(models);
        return models;
    }

    public static async Task<RoleQueryModel> AssignRoleAsync(
        this HttpClient client,
        string userId,
        CreateRoleAssignmentCommandModel command,
        CancellationToken cancel)
    {
        var content = JsonContent.Create(command);
        var response = await client.PostAsync($"users/{userId}/roles", content, cancel);
        response.EnsureSuccessStatusCode();
        var model = await response.Content.ReadFromJsonAsync<RoleQueryModel>(cancellationToken: cancel);
        Assert.NotNull(model);
        return model;
    }

    public static async Task<List<RoleQueryModel>> GetRolesForUserAsync(
        this HttpClient client,
        string userId,
        CancellationToken cancel)
    {
        var response = await client.GetAsync($"users/{userId}/roles", cancel);
        response.EnsureSuccessStatusCode();
        var models = await response.Content.ReadFromJsonAsync<List<RoleQueryModel>>(cancellationToken: cancel);
        Assert.NotNull(models);
        return models;
    }
}
