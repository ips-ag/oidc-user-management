using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework;

public interface IDataSeed
{
    IReadOnlyCollection<ApplicationRoleModel> GetRoles();
    IReadOnlyCollection<ApplicationUserModel> GetUsers();
    IReadOnlyCollection<ApplicationRoleClaim> GetRoleClaims();
}
