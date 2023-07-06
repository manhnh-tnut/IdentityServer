﻿using Duende.IdentityServer.EntityFramework.Entities;
using IdentityServer.Infrastructure.Common;

namespace IdentityServer.Infrastructure.Repositories.Interfaces;
public interface IKeyRepository
{
    Task<PagedList<Key>> GetKeysAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<Key> GetKeyAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> ExistsKeyAsync(string id, CancellationToken cancellationToken = default);
    Task DeleteKeyAsync(string id, CancellationToken cancellationToken = default);
    Task<int> SaveAllChangesAsync(CancellationToken cancellationToken = default);
    bool AutoSaveChanges { get; set; }
}