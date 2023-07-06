using IdentityServer.Features.Keys.Mappers;
using IdentityServer.Features.Keys.Models;
using IdentityServer.Features.Keys.Resources.Interfaces;
using IdentityServer.Infrastructure.Exceptions;
using IdentityServer.Infrastructure.Repositories.Interfaces;

namespace IdentityServer.Features.Keys.Services;

public class KeyService : Interfaces.IKeyService
{
    protected readonly IKeyRepository KeyRepository;
    protected readonly IKeyServiceResources KeyServiceResources;

    public KeyService(IKeyRepository keyRepository
    , IKeyServiceResources keyServiceResources)
    {
        KeyRepository = keyRepository;
        KeyServiceResources = keyServiceResources;
    }

    public async Task<KeysDto> GetKeysAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var keys = await KeyRepository.GetKeysAsync(page, pageSize, cancellationToken);

        var keysDto = keys.ToModel();

        return keysDto;
    }

    public async Task<KeyDto> GetKeyAsync(string id, CancellationToken cancellationToken = default)
    {
        var key = await KeyRepository.GetKeyAsync(id, cancellationToken);

        if (key == default)
        {
            throw new UserFriendlyErrorPageException(string.Format(KeyServiceResources.KeyDoesNotExist().Description, id));
        }

        var keyDto = key.ToModel();

        return keyDto;
    }

    public Task<bool> ExistsKeyAsync(string id, CancellationToken cancellationToken = default)
    {
        return KeyRepository.ExistsKeyAsync(id, cancellationToken);
    }

    public async Task DeleteKeyAsync(string id, CancellationToken cancellationToken = default)
    {
        var key = await GetKeyAsync(id, cancellationToken);

        await KeyRepository.DeleteKeyAsync(id, cancellationToken);
    }
}