﻿
using Duende.IdentityServer.EntityFramework.Entities;
using IdentityServer.Infrastructure.Common;

namespace IdentityServer.Infrastructure.Repositories.Interfaces;
public interface IPersistedGrantRepository
{
    Task<PagedList<Entities.PersistedGrantDataView>> GetPersistedGrantsByUsersAsync(string search, int page = 1, int pageSize = 10);
    Task<PagedList<PersistedGrant>> GetPersistedGrantsByUserAsync(string subjectId, int page = 1, int pageSize = 10);
    Task<PersistedGrant> GetPersistedGrantAsync(string key);
    Task<int> DeletePersistedGrantAsync(string key);
    Task<int> DeletePersistedGrantsAsync(string userId);
    Task<bool> ExistsPersistedGrantsAsync(string subjectId);
    Task<bool> ExistsPersistedGrantAsync(string key);
    Task<int> SaveAllChangesAsync();
    bool AutoSaveChanges { get; set; }
}