using IdentityServer.Features.Shared.Models;

namespace IdentityServer.Features.PersistedGrants.Resources;

public class PersistedGrantServiceResources : Interfaces.IPersistedGrantServiceResources
{
    public virtual ResourceMessage PersistedGrantDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(PersistedGrantDoesNotExist),
            Description = "Persisted Grant with id {0} doesn't exist"
        };
    }

    public virtual ResourceMessage PersistedGrantWithSubjectIdDoesNotExist()
    {
        return new ResourceMessage()
        {
            Code = nameof(PersistedGrantWithSubjectIdDoesNotExist),
            Description = "Persisted Grant for this subject id {0} doesn't exist"
        };
    }
}