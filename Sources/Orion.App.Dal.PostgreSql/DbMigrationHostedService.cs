using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Orion.App.Dal.PostgreSql;

public sealed class DbMigrationHostedService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<DbMigrationHostedService> _logger;
    public DbMigrationHostedService(IServiceScopeFactory serviceScopeFactory, ILogger<DbMigrationHostedService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("DB migrations apply in progress...");

        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var dbContext = scope.ServiceProvider.GetRequiredService<OrionDbContext>();
            await dbContext.Migrate(cancellationToken);

            _logger.LogInformation("DB migrations have been applied");
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Unable to apply DB migrations");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}