using IdentityServer.Features.Shared.Models;

namespace IdentityServer.Features.Keys.Resources.Interfaces;

public interface IKeyServiceResources
{
    ResourceMessage KeyDoesNotExist();
}