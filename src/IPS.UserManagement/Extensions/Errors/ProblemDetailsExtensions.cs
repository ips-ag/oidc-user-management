using Hellang.Middleware.ProblemDetails;

namespace IPS.UserManagement.Extensions.Errors;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddProblemDetailsServices(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddSingleton<IConfigureOptions<ProblemDetailsOptions>, ConfigureProblemDetailsOptions>();
        return services;
    }
}
