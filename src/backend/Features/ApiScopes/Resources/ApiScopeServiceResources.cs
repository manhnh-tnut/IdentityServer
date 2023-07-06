using IdentityServer.Features.Shared.Models;
using IdentityServer.Features.ApiScopes.Resources.Interfaces;

namespace IdentityServer.Features.ApiScopes.Resources;

public class ApiScopeServiceResources : IApiScopeServiceResources
{
    public virtual ResourceMessage ApiScopeDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiScopeDoesNotExist),
            Description = "Api Scope with id {0} doesn't exist"
        };
    }

    public virtual ResourceMessage ApiScopeExistsValue()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiScopeExistsValue),
            Description = "Api Scope ({0}) already exists"
        };
    }

    public virtual ResourceMessage ApiScopeExistsKey()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiScopeExistsKey),
            Description = "Api Scope already exists"
        };
    }

    public ResourceMessage ApiScopePropertyExistsValue()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiScopePropertyExistsValue),
            Description = "Api Scope Property ({0}) already exists"
        };
    }

    public ResourceMessage ApiScopePropertyDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiScopePropertyDoesNotExist),
            Description = "Api Scope Property with id {0} doesn't exist"
        };
    }

    public ResourceMessage ApiScopePropertyExistsKey()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiScopePropertyExistsKey),
            Description = "Api Scope Property already exists"
        };
    }
}