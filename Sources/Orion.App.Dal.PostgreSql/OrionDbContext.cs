using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Orion.App.Dal.PostgreSql.Entities;

namespace Orion.App.Dal.PostgreSql;

public class OrionDbContext: DbContext
{
    internal virtual DbSet<FeedDal> Feeds { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeedDal>(FeedDal.ConfigureModel);
        base.OnModelCreating(modelBuilder);
    }
    
    public async Task Migrate(CancellationToken cancellationToken)
    {
        var migrator = this.GetService<IMigrator>(); 
        await migrator.MigrateAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}