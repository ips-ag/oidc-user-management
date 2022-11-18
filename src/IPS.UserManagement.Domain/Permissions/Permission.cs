using IPS.UserManagement.Domain.Resources;

namespace IPS.UserManagement.Domain.Permissions;

public record Permission(string Id, string Name, string Description, Resource Resource);
