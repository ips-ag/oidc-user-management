using Microsoft.Extensions.DependencyInjection;

namespace IPS.UserManagement.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.AsScoped(), typeof(ApplicationExtensions).Assembly);
        return services;
    }
}
