using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityServer.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data;

public class AdminPersistedGrantDbContext : PersistedGrantDbContext<AdminPersistedGrantDbContext>, IAdminPersistedGrantDbContext
{
    public AdminPersistedGrantDbContext(DbContextOptions<AdminPersistedGrantDbContext> options)
        : base(options)
    {
    }
}