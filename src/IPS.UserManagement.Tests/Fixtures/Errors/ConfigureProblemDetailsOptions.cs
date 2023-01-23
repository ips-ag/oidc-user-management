using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.Options;

namespace IPS.UserManagement.Tests.Fixtures.Errors;

public class ConfigureProblemDetailsOptions : IPostConfigureOptions<ProblemDetailsOptions>
{
    public void PostConfigure(string? name, ProblemDetailsOptions options)
    {
        options.IncludeExceptionDetails = (_, _) => true;
        options.ShouldLogUnhandledException = (_, _, _) => true;
    }
}
