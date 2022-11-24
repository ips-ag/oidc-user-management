using Hellang.Middleware.ProblemDetails;
using IPS.UserManagement.Domain.Exceptions;

namespace IPS.UserManagement.Extensions.Errors;

public class ConfigureProblemDetailsOptions : IConfigureOptions<ProblemDetailsOptions>
{
    private readonly IHostEnvironment _environment;

    public ConfigureProblemDetailsOptions(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public void Configure(ProblemDetailsOptions options)
    {
        options.IncludeExceptionDetails = (_, _) => _environment.IsDevelopment();
        // framework
        options.MapToStatusCode<OperationCanceledException>(StatusCodes.Status408RequestTimeout);
        options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
        options.MapToStatusCode<ArgumentException>(StatusCodes.Status422UnprocessableEntity);
        // domain
        options.MapToStatusCode<EntityNotFoundException>(StatusCodes.Status404NotFound);
        // dependencies
        options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
        // fallback
        options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        options.ShouldLogUnhandledException = (_, _, detail) => detail.Status is >= 500;
    }
}
