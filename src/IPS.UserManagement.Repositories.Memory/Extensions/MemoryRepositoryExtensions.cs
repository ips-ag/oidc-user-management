using IPS.UserManagement.Domain.Resources;
using IPS.UserManagement.Repositories.Memory.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace IPS.UserManagement.Repositories.Memory.Extensions;

public static class MemoryRepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IResourceRepository, ResourceRepository>();
        return services;
    }
}
