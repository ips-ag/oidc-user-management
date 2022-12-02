using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IPS.UserManagement.Repositories.AspNetCoreIdentity.Extensions;

public class ConfigureIdentityOptions : IConfigureOptions<IdentityOptions>
{
    public void Configure(IdentityOptions options)
    {
        options.SignIn.RequireConfirmedAccount = true;
    }
}
