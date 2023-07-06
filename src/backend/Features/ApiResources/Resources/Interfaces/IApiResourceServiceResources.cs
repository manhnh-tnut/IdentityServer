using IdentityServer.Features.Shared.Models;

namespace IdentityServer.Features.ApiResources.Resources.Interfaces;
public interface IApiResourceServiceResources
{
    ResourceMessage ApiResourceDoesNotExist();
    ResourceMessage ApiResourceExistsValue();
    ResourceMessage ApiResourceExistsKey();
    ResourceMessage ApiSecretDoesNotExist();
    ResourceMessage ApiResourcePropertyDoesNotExist();
    ResourceMessage ApiResourcePropertyExistsKey();
    ResourceMessage ApiResourcePropertyExistsValue();
}