using IdentityServer.Features.Shared.Models;

namespace IdentityServer.Features.Clients.Resources;

public class ClientServiceResources : Interfaces.IClientServiceResources
{
    public virtual ResourceMessage ClientClaimDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(ClientClaimDoesNotExist),
            Description = "Client claim with id {0} doesn't exist"
        };
    }

    public virtual ResourceMessage ClientDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(ClientDoesNotExist),
            Description = "Client with id {0} doesn't exist"
        };
    }

    public virtual ResourceMessage ClientExistsKey()
    {
        return new ResourceMessage()
        {
            Code = nameof(ClientExistsKey),
            Description = "Client already exists"
        };
    }

    public virtual ResourceMessage ClientExistsValue()
    {
        return new ResourceMessage()
        {
            Code = nameof(ClientExistsValue),
            Description = "Client Id ({0}) already exists"
        };
    }

    public virtual ResourceMessage ClientPropertyDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(ClientPropertyDoesNotExist),
            Description = "Client property with id {0} doesn't exist"
        };
    }

    public virtual ResourceMessage ClientSecretDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(ClientSecretDoesNotExist),
            Description = "Client secret with id {0} doesn't exist"
        };
    }
}