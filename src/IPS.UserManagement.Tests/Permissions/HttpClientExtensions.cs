using System.Net.Http.Json;
using IPS.UserManagement.Application.Features.Permissions.Models;
using IPS.UserManagement.Application.Features.Resources.Models;

namespace IPS.UserManagement.Tests.Permissions;

public static class HttpClientExtensions
{
    public static async Task<PermissionQueryModel> CreatePermissionAsync(
        this HttpClient client,
        CreatePermissionCommandModel command,
        CancellationToken cancel)
    {
        var content = JsonContent.Create(command);
        var response = await client.PostAsync("permissions", content, cancel);
        response.EnsureSuccessStatusCode();
        var model = await response.Content.ReadFromJsonAsync<PermissionQueryModel>(cancellationToken: cancel);
        Assert.NotNull(model);
        return model;
    }

    public static async Task<List<ResourceQueryModel>> GetPermissionsForResourceAsync(
        this HttpClient client,
        string resourceId,
        CancellationToken cancel)
    {
        var response = await client.GetAsync($"permissions?resource={resourceId}", cancel);
        response.EnsureSuccessStatusCode();
        var models = await response.Content.ReadFromJsonAsync<List<ResourceQueryModel>>(cancellationToken: cancel);
        Assert.NotNull(models);
        return models;
    }
}
