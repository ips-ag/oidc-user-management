using IPS.UserManagement.Application.Features.Users.Converters;
using IPS.UserManagement.Application.Features.Users.Models;
using IPS.UserManagement.Domain.Users;

namespace IPS.UserManagement.Application.Features.Users.Commands;

public class GetUsersCommand : IRequest<List<UserQueryModel>>
{
    private string? Id { get; }
    private string? UserName { get; }

    public GetUsersCommand(string? id, string? userName)
    {
        Id = id;
        UserName = userName;
    }

    private class GetUsersCommandHandler : IRequestHandler<GetUsersCommand, List<UserQueryModel>>
    {
        private readonly IUserRepository _repository;
        private readonly UserConverter _converter;

        public GetUsersCommandHandler(IUserRepository repository, UserConverter converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public async Task<List<UserQueryModel>> Handle(GetUsersCommand command, CancellationToken cancel)
        {
            QueryRequest request = new(command.Id, command.UserName);
            var users = await _repository.QueryAsync(request, cancel);
            return _converter.ToModel(users);
        }
    }
}
