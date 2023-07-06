using System.Linq.Expressions;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Infrastructure.Common;
using IdentityServer.Infrastructure.Entities;
using IdentityServer.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IdentityServer.Infrastructure.Repositories;
public class PersistedGrantRepository<TIdentityDbContext, TPersistedGrantDbContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : Interfaces.IPersistedGrantRepository
        where TIdentityDbContext : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TPersistedGrantDbContext : DbContext, IPersistedGrantDbContext
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
{
    protected readonly TIdentityDbContext _identityDbContext;
    protected readonly TPersistedGrantDbContext _persistedGrantDbContext;

    public bool AutoSaveChanges { get; set; } = true;

    public PersistedGrantRepository(TIdentityDbContext identityDbContext, TPersistedGrantDbContext persistedGrantDbContext)
    {
        _identityDbContext = identityDbContext;
        _persistedGrantDbContext = persistedGrantDbContext;
    }

    public virtual Task<PagedList<PersistedGrantDataView>> GetPersistedGrantsByUsersAsync(string search, int page = 1, int pageSize = 10)
    {
        return Task.Run(() =>
        {
            var pagedList = new PagedList<PersistedGrantDataView>();

            var persistedGrantByUsers = (from pe in _persistedGrantDbContext.PersistedGrants.ToList()
                                         join us in _identityDbContext.Users.ToList() on pe.SubjectId equals us.Id.ToString() into per
                                         from identity in per.DefaultIfEmpty()
                                         select new PersistedGrantDataView
                                         {
                                             SubjectId = pe.SubjectId,
                                             SubjectName = identity == null ? string.Empty : identity.UserName
                                         })
                .GroupBy(x => x.SubjectId).Select(g => g.First());

            if (!string.IsNullOrEmpty(search))
            {
                Expression<Func<PersistedGrantDataView, bool>> searchCondition = x => x.SubjectId.Contains(search) || x.SubjectName.Contains(search);
                Func<PersistedGrantDataView, bool> searchPredicate = searchCondition.Compile();
                persistedGrantByUsers = persistedGrantByUsers.Where(searchPredicate);
            }

            var persistedGrantDataViews = persistedGrantByUsers.ToList();

            var persistedGrantsData = persistedGrantDataViews.AsQueryable().PageBy(x => x.SubjectId, page, pageSize).ToList();
            var persistedGrantsDataCount = persistedGrantDataViews.Count;

            pagedList.Data.AddRange(persistedGrantsData);
            pagedList.TotalCount = persistedGrantsDataCount;
            pagedList.PageSize = pageSize;

            return pagedList;
        });
    }

    public virtual async Task<PagedList<PersistedGrant>> GetPersistedGrantsByUserAsync(string subjectId, int page = 1, int pageSize = 10)
    {
        var pagedList = new PagedList<PersistedGrant>();

        var persistedGrantsData = await _persistedGrantDbContext.PersistedGrants.Where(x => x.SubjectId == subjectId).Select(x => new PersistedGrant()
        {
            SubjectId = x.SubjectId,
            Type = x.Type,
            Key = x.Key,
            ClientId = x.ClientId,
            Data = x.Data,
            Expiration = x.Expiration,
            CreationTime = x.CreationTime
        }).PageBy(x => x.SubjectId, page, pageSize).ToListAsync();

        var persistedGrantsCount = await _persistedGrantDbContext.PersistedGrants.Where(x => x.SubjectId == subjectId).CountAsync();

        pagedList.Data.AddRange(persistedGrantsData);
        pagedList.TotalCount = persistedGrantsCount;
        pagedList.PageSize = pageSize;

        return pagedList;
    }

    public virtual Task<PersistedGrant> GetPersistedGrantAsync(string key)
    {
        return _persistedGrantDbContext.PersistedGrants.SingleOrDefaultAsync(x => x.Key == key);
    }

    public virtual async Task<int> DeletePersistedGrantAsync(string key)
    {
        var persistedGrant = await _persistedGrantDbContext.PersistedGrants.Where(x => x.Key == key).SingleOrDefaultAsync();

        _persistedGrantDbContext.PersistedGrants.Remove(persistedGrant);

        return await AutoSaveChangesAsync();
    }

    public virtual Task<bool> ExistsPersistedGrantsAsync(string subjectId)
    {
        return _persistedGrantDbContext.PersistedGrants.AnyAsync(x => x.SubjectId == subjectId);
    }

    public Task<bool> ExistsPersistedGrantAsync(string key)
    {
        return _persistedGrantDbContext.PersistedGrants.AnyAsync(x => x.Key == key);
    }

    public virtual async Task<int> DeletePersistedGrantsAsync(string userId)
    {
        var grants = await _persistedGrantDbContext.PersistedGrants.Where(x => x.SubjectId == userId).ToListAsync();

        _persistedGrantDbContext.RemoveRange(grants);

        return await AutoSaveChangesAsync();
    }

    protected virtual async Task<int> AutoSaveChangesAsync()
    {
        return AutoSaveChanges ? await _persistedGrantDbContext.SaveChangesAsync() : (int)Enums.SavedStatus.WillBeSavedExplicitly;
    }

    public virtual async Task<int> SaveAllChangesAsync()
    {
        return await _persistedGrantDbContext.SaveChangesAsync();
    }
}