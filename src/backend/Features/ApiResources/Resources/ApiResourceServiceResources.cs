using IdentityServer.Features.Shared.Models;
using IdentityServer.Features.ApiResources.Resources.Interfaces;

namespace IdentityServer.Features.ApiResources.Resources;

public class ApiResourceServiceResources : IApiResourceServiceResources
{
    public virtual ResourceMessage ApiResourceDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiResourceDoesNotExist),
            Description = "Api Resource with id {0} doesn't exist"
        };
    }

    public virtual ResourceMessage ApiResourceExistsKey()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiResourceExistsKey),
            Description = "Api Resource already exists"
        };
    }

    public virtual ResourceMessage ApiResourceExistsValue()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiResourceExistsValue),
            Description = "Api Resource ({0}) already exists"
        };
    }

    public virtual ResourceMessage ApiSecretDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiSecretDoesNotExist),
            Description = "Api Secret with id {0} doesn't exist"
        };
    }

    public virtual ResourceMessage ApiResourcePropertyDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiResourcePropertyDoesNotExist),
            Description = "Api Resource Property with id {0} doesn't exist"
        };
    }

    public virtual ResourceMessage ApiResourcePropertyExistsKey()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiResourcePropertyExistsKey),
            Description = "Api Resource Property already exists"
        };
    }

    public virtual ResourceMessage ApiResourcePropertyExistsValue()
    {
        return new ResourceMessage()
        {
            Code = nameof(ApiResourcePropertyExistsValue),
            Description = "Api Resource Property with key ({0}) already exists"
        };
    }
}