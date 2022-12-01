using IPS.UserManagement.Application.Features.Users.Models;
using IPS.UserManagement.Domain.Users;

namespace IPS.UserManagement.Application.Features.Users.Converters;

internal class UserConverter
{
    public List<UserQueryModel> ToModel(IReadOnlyCollection<User> users)
    {
        return users.Select(ToModel).ToList();
    }

    private UserQueryModel ToModel(User user)
    {
        return new UserQueryModel { Id = user.Id, UserName = user.Name, Email = user.Email };
    }
}
