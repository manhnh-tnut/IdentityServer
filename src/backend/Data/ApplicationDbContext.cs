using IdentityServer.Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=IdentityServer.db;");
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityRole>().ToTable(TableConstants.IdentityRoles);
        builder.Entity<IdentityRoleClaim<string>>().ToTable(TableConstants.IdentityRoleClaims);
        builder.Entity<IdentityUserRole<string>>().ToTable(TableConstants.IdentityUserRoles);
        builder.Entity<ApplicationUser>().ToTable(TableConstants.IdentityUsers);
        builder.Entity<IdentityUserLogin<string>>().ToTable(TableConstants.IdentityUserLogins);
        builder.Entity<IdentityUserClaim<string>>().ToTable(TableConstants.IdentityUserClaims);
        builder.Entity<IdentityUserToken<string>>().ToTable(TableConstants.IdentityUserTokens);
    }
}