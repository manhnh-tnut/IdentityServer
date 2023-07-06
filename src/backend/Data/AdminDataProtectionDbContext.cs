using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data;

public class AdminDataProtectionDbContext : DbContext, IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public AdminDataProtectionDbContext(DbContextOptions<AdminDataProtectionDbContext> options)
        : base(options) { }
}