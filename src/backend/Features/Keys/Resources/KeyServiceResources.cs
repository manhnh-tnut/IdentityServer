using IdentityServer.Features.Shared.Models;

namespace IdentityServer.Features.Keys.Resources;

public class KeyServiceResources : Interfaces.IKeyServiceResources
{
    public ResourceMessage KeyDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(KeyDoesNotExist),
            Description = "Key with id {0} doesn't exist"
        };
    }
}