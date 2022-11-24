using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace IPS.UserManagement.Extensions.Errors;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddProblemDetailsServices(this IServiceCollection services)
    {
        Hellang.Middleware.ProblemDetails.ProblemDetailsExtensions.AddProblemDetails(services);
        services.AddSingleton<IConfigureOptions<ProblemDetailsOptions>, ConfigureProblemDetailsOptions>();
        return services;
    }
}
