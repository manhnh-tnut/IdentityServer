using IdentityServer.Features.Shared.Models;

namespace IdentityServer.Features.Clients.Resources.Interfaces;

public interface IClientServiceResources
{
    ResourceMessage ClientClaimDoesNotExist();

    ResourceMessage ClientDoesNotExist();

    ResourceMessage ClientExistsKey();

    ResourceMessage ClientExistsValue();

    ResourceMessage ClientPropertyDoesNotExist();

    ResourceMessage ClientSecretDoesNotExist();
}