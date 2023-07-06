
using IdentityServer.Features.Shared.Models;

namespace IdentityServer.Features.IdentityResources.Resources.Interfaces;

public interface IIdentityResourceServiceResources
{
    ResourceMessage IdentityResourceDoesNotExist();

    ResourceMessage IdentityResourceExistsKey();

    ResourceMessage IdentityResourceExistsValue();

    ResourceMessage IdentityResourcePropertyDoesNotExist();

    ResourceMessage IdentityResourcePropertyExistsValue();

    ResourceMessage IdentityResourcePropertyExistsKey();
}