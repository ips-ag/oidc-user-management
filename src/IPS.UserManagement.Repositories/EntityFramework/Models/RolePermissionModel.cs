#pragma warning disable CS8618

namespace IPS.UserManagement.Repositories.EntityFramework.Models;

public class RolePermissionModel
{
    public int Id { get; set; }
    public string Permission { get; set; }
    public int RoleId { get; set; }
    public RoleModel Role { get; set; }
}
