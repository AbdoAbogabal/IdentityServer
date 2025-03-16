namespace identityServerWeb.DBContext.PersistedGrantDbContext;

using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

public class PersistedGrantAppDbContext : PersistedGrantDbContext<PersistedGrantAppDbContext>
{
    public PersistedGrantAppDbContext(DbContextOptions<PersistedGrantAppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}