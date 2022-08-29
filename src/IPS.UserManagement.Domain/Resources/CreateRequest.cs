namespace IPS.UserManagement.Domain.Resources;

public record struct CreateRequest(string Name, string? Description, string Location);
