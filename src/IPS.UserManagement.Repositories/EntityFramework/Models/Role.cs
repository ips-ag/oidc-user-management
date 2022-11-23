#pragma warning disable CS8618

namespace IPS.UserManagement.Repositories.EntityFramework.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Updated { get; set; }
    public DateTime? LastAccessed { get; set; }
}
