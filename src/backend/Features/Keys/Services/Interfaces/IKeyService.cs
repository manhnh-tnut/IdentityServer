using IdentityServer.Features.Keys.Models;
namespace IdentityServer.Features.Keys.Services.Interfaces;
public interface IKeyService
{
    Task<KeysDto> GetKeysAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<KeyDto> GetKeyAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> ExistsKeyAsync(string id, CancellationToken cancellationToken = default);
    Task DeleteKeyAsync(string id, CancellationToken cancellationToken = default);
}