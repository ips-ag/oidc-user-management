using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework;

internal class DataSeed : IDataSeed
{
    public IReadOnlyCollection<ApplicationRoleModel> GetRoles()
    {
        return new List<ApplicationRoleModel>();
    }

    public IReadOnlyCollection<ApplicationUserModel> GetUsers()
    {
        return new List<ApplicationUserModel>();
    }

    public IReadOnlyCollection<ApplicationRoleClaim> GetRoleClaims()
    {
        return new List<ApplicationRoleClaim>();
    }
}
