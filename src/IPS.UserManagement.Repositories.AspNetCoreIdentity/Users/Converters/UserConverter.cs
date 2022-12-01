using IPS.UserManagement.Domain.Users;
using IPS.UserManagement.Repositories.AspNetCoreIdentity.EntityFramework.Models;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.Users.Converters;

internal class UserConverter
{
    public User ToDomain(ApplicationUserModel model)
    {
        if (model.UserName is null || model.Email is null)
        {
            throw new ArgumentException($"User {model.Id} is missing username or email");
        }
        return new User(model.Id, model.UserName, model.Email);
    }
}
