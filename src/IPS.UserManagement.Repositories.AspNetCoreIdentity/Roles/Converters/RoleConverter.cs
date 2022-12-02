using IPS.UserManagement.Domain.Roles;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;
using Microsoft.AspNetCore.Identity;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.Roles.Converters;

internal class RoleConverter
{
    private readonly ILookupNormalizer _normalizer;

    public RoleConverter(ILookupNormalizer normalizer)
    {
        _normalizer = normalizer;
    }

    public ApplicationRoleModel ToModel(CreateRequest request)
    {
        return new ApplicationRoleModel
        {
            Name = request.Name,
            NormalizedName = _normalizer.NormalizeName(request.Name),
            Description = request.Description
        };
    }

    public Role ToDomain(ApplicationRoleModel role)
    {
        return new Role(role.Id, role.Name, role.Description);
    }

    public IReadOnlyCollection<Role> ToDomain(IEnumerable<string> roleModels)
    {
        throw new NotImplementedException();
    }
}
