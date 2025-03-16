namespace identityServerWeb.DBContext.ConfigurationDbContext;

using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

public class ConfigurationAppDbContext : ConfigurationDbContext<ConfigurationAppDbContext>
{
    public ConfigurationAppDbContext(DbContextOptions<ConfigurationAppDbContext> options)
        : base(options) // This is correct because ConfigurationDbContext<TContext> expects DbContextOptions<TContext>
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}