using IdentityServer.Features.Shared.Models;

namespace IdentityServer.Features.PersistedGrants.Resources.Interfaces;

public interface IPersistedGrantServiceResources
{
    ResourceMessage PersistedGrantDoesNotExist();

    ResourceMessage PersistedGrantWithSubjectIdDoesNotExist();
}