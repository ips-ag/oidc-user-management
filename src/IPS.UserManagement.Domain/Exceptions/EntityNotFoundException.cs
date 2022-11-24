namespace IPS.UserManagement.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type entityType, object entityId) : base($"{entityType.Name} {entityId} not found")
    {
    }
}
