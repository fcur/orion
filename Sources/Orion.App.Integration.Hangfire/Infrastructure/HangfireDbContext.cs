using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orion.App.Integration.Hangfire.Infrastructure;

public sealed class HangfireDbContext: DbContext
{
    public HangfireDbContext(DbContextOptions<HangfireDbContext> options)
        : base(options)
    {
    }
    
    public void Migrate()
    {
        this
            .GetService<IMigrator>()
            .Migrate();
    }
}