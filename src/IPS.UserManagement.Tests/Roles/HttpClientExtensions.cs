using System.Net.Http.Json;
using IPS.UserManagement.Application.Features.Roles.Models;

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
}
