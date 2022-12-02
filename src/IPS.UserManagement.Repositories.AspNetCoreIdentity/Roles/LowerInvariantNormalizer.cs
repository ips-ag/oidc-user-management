using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.Roles;

internal class LowerInvariantNormalizer : ILookupNormalizer
{
    [return: NotNullIfNotNull(nameof(name))]
    public string? NormalizeName(string? name)
    {
        return name?.ToLowerInvariant();
    }

    [return: NotNullIfNotNull(nameof(email))]
    public string? NormalizeEmail(string? email)
    {
        return email?.ToLowerInvariant();
    }
}
