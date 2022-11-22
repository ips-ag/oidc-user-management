using IPS.UserManagement.Application.Features.Roles.Models;

namespace IPS.UserManagement.Application.Features.Roles.Commands;

public class CreateRoleCommand : IRequest<RoleQueryModel>
{
    private CreateRoleCommandModel Request { get; }

    public CreateRoleCommand(CreateRoleCommandModel request)
    {
        Request = request;
    }
    
    private class CreateRoleCommandHandler: IRequestHandler<CreateRoleCommand, RoleQueryModel>
    {
        public Task<RoleQueryModel> Handle(CreateRoleCommand request, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }
    }
}
