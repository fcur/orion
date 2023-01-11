using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orion.App.Dal.PostgreSql;
using Orion.App.Dal.PostgreSql.Repositories;
using Orion.App.Domain.FeedEntity;

namespace Orion.App.Dal.PostgreSQL.Infrastructure;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigurePostgreSQL(this IServiceCollection services, IConfiguration configuration)
    {
        // todo: add health checks
        services.AddDbContextPool<OrionDbContext>(opt =>
        {
            opt.EnableDetailedErrors();

            var connectionString = configuration.GetConnectionString("PostgreSql");

            opt.UseNpgsql(connectionString);
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddHostedService<DbMigrationHostedService>();

        services.AddTransient<IFeedRepository, FeedRepository>();

        return services;
    }
}
