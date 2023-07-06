using IdentityServer.Features.PersistedGrants.Mappers;
using IdentityServer.Features.PersistedGrants.Models;
using IdentityServer.Features.PersistedGrants.Resources.Interfaces;
using IdentityServer.Features.PersistedGrants.Services.Interfaces;
using IdentityServer.Infrastructure.Exceptions;
using IdentityServer.Infrastructure.Repositories.Interfaces;

namespace IdentityServer.Features.PersistedGraViewModelnts.Services;

public class PersistedGrantService : IPersistedGrantService
{
    protected readonly IPersistedGrantRepository _persistedGrantRepository;
    protected readonly IPersistedGrantServiceResources _persistedGrantServiceResources;

    public PersistedGrantService(IPersistedGrantRepository persistedGrantRepository,
        IPersistedGrantServiceResources persistedGrantServiceResources)
    {
        _persistedGrantRepository = persistedGrantRepository;
        _persistedGrantServiceResources = persistedGrantServiceResources;
    }

    public virtual async Task<PersistedGrantsDto> GetPersistedGrantsByUsersAsync(string search, int page = 1, int pageSize = 10)
    {
        var pagedList = await _persistedGrantRepository.GetPersistedGrantsByUsersAsync(search, page, pageSize);
        var persistedGrantsDto = pagedList.ToModel();

        return persistedGrantsDto;
    }

    public virtual async Task<PersistedGrantsDto> GetPersistedGrantsByUserAsync(string subjectId, int page = 1, int pageSize = 10)
    {
        var exists = await _persistedGrantRepository.ExistsPersistedGrantsAsync(subjectId);
        if (!exists) throw new UserFriendlyErrorPageException(string.Format(_persistedGrantServiceResources.PersistedGrantWithSubjectIdDoesNotExist().Description, subjectId), _persistedGrantServiceResources.PersistedGrantWithSubjectIdDoesNotExist().Description);

        var pagedList = await _persistedGrantRepository.GetPersistedGrantsByUserAsync(subjectId, page, pageSize);
        var persistedGrantsDto = pagedList.ToModel();

        return persistedGrantsDto;
    }

    public virtual async Task<PersistedGrantDto> GetPersistedGrantAsync(string key)
    {
        var persistedGrant = await _persistedGrantRepository.GetPersistedGrantAsync(key);
        if (persistedGrant == null) throw new UserFriendlyErrorPageException(string.Format(_persistedGrantServiceResources.PersistedGrantDoesNotExist().Description, key), _persistedGrantServiceResources.PersistedGrantDoesNotExist().Description);
        var persistedGrantDto = persistedGrant.ToModel();

        return persistedGrantDto;
    }

    public virtual async Task<int> DeletePersistedGrantAsync(string key)
    {
        var exists = await _persistedGrantRepository.ExistsPersistedGrantAsync(key);
        if (!exists) throw new UserFriendlyErrorPageException(string.Format(_persistedGrantServiceResources.PersistedGrantDoesNotExist().Description, key), _persistedGrantServiceResources.PersistedGrantDoesNotExist().Description);

        var deleted = await _persistedGrantRepository.DeletePersistedGrantAsync(key);

        return deleted;
    }

    public virtual async Task<int> DeletePersistedGrantsAsync(string userId)
    {
        var exists = await _persistedGrantRepository.ExistsPersistedGrantsAsync(userId);
        if (!exists) throw new UserFriendlyErrorPageException(string.Format(_persistedGrantServiceResources.PersistedGrantWithSubjectIdDoesNotExist().Description, userId), _persistedGrantServiceResources.PersistedGrantWithSubjectIdDoesNotExist().Description);

        var deleted = await _persistedGrantRepository.DeletePersistedGrantsAsync(userId);

        return deleted;
    }
}