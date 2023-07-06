using IdentityServer.Features.Shared.Models;

namespace IdentityServer.Features.IdentityResources.Resources;

public class IdentityResourceServiceResources : Interfaces.IIdentityResourceServiceResources
{
    public virtual ResourceMessage IdentityResourceDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(IdentityResourceDoesNotExist),
            Description = "Identity Resource with id {0} doesn't exist"
        };
    }

    public virtual ResourceMessage IdentityResourceExistsKey()
    {
        return new ResourceMessage()
        {
            Code = nameof(IdentityResourceExistsKey),
            Description = "Identity Resource {0} already exists"
        };
    }

    public virtual ResourceMessage IdentityResourceExistsValue()
    {
        return new ResourceMessage()
        {
            Code = nameof(IdentityResourceExistsValue),
            Description = "Identity Resource {0} already exists"
        };
    }

    public virtual ResourceMessage IdentityResourcePropertyDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(IdentityResourcePropertyDoesNotExist),
            Description = "Identity Resource Property with id {0} doesn't exist"
        };
    }

    public virtual ResourceMessage IdentityResourcePropertyExistsValue()
    {
        return new ResourceMessage()
        {
            Code = nameof(IdentityResourcePropertyExistsValue),
            Description = "Identity Resource Property with key ({0}) already exists"
        };
    }

    public virtual ResourceMessage IdentityResourcePropertyExistsKey()
    {
        return new ResourceMessage()
        {
            Code = nameof(IdentityResourcePropertyExistsKey),
            Description = "Identity Resource Property already exists"
        };
    }
}