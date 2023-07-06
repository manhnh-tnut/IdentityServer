using IdentityServer.Features.Shared.Models;
namespace IdentityServer.Features.ApiScopes.Resources.Interfaces;

public interface IApiScopeServiceResources
{
    ResourceMessage ApiScopeDoesNotExist();
    ResourceMessage ApiScopeExistsValue();
    ResourceMessage ApiScopeExistsKey();
    ResourceMessage ApiScopePropertyExistsValue();
    ResourceMessage ApiScopePropertyDoesNotExist();
    ResourceMessage ApiScopePropertyExistsKey();
}