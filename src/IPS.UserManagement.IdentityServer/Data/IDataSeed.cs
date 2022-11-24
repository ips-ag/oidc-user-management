using Duende.IdentityServer.Models;

namespace IPS.UserManagement.IdentityServer.Data;

public interface IDataSeed
{
    IEnumerable<Client> GetClients();
    IEnumerable<IdentityResource> GetIdentityResources();
    IEnumerable<ApiScope> GetScopes();
    IEnumerable<ApiResource> GetApis();
}
