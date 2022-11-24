namespace IPS.UserManagement.Domain.Permissions;

public record CreateRequest(string Name, string? Description, string Resource);
