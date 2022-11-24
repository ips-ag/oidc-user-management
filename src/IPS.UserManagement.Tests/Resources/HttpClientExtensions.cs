using System.Net.Http.Json;
using IPS.UserManagement.Application.Features.Resources.Models;

namespace IPS.UserManagement.Tests.Resources;

public static class HttpClientExtensions
{
    public static async Task<ResourceQueryModel> CreateResourceAsync(
        this HttpClient client,
        CreateResourceCommandModel command,
        CancellationToken cancel)
    {
        var content = JsonContent.Create(command);
        var response = await client.PostAsync("resources", content, cancel);
        response.EnsureSuccessStatusCode();
        var model = await response.Content.ReadFromJsonAsync<ResourceQueryModel>(cancellationToken: cancel);
        Assert.NotNull(model);
        return model;
    }

    public static async Task<List<ResourceQueryModel>> GetResourcesAsync(
        this HttpClient client,
        CancellationToken cancel)
    {
        var response = await client.GetAsync("resources", cancel);
        response.EnsureSuccessStatusCode();
        var models = await response.Content.ReadFromJsonAsync<List<ResourceQueryModel>>(cancellationToken: cancel);
        Assert.NotNull(models);
        return models;
    }
}
