using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Orion.App.Integration.Hangfire.Infrastructure;

public static class HangfireExtensions
{
    public static IServiceCollection ConfigureHangfire(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var hangfireDbConnectionString = configuration.GetConnectionString("PostgreSql_Hangfire");
        services.ConfigureDatabase(hangfireDbConnectionString!, "Orion.App.Integration.Hangfire");

        services.AddHangfire((v, u) =>
        {
            var dbContext = v.GetRequiredService<HangfireDbContext>();
            dbContext.Migrate();
            u.UsePostgreSqlStorage(hangfireDbConnectionString);
        });

        services.AddHangfireServer(v =>
        {
            v.ServerName = "local";
        });
        
        return services;
    }

    public static IApplicationBuilder AddRecurringJob<TJob>(this IApplicationBuilder app, string settingsSectionName)
        where TJob : IHangfireJob
    {
        var recurringJobManager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();
        var jobSettings = app.ApplicationServices.GetRequiredService<IConfiguration>().GetSection(settingsSectionName).Get<RecurringJobSettings>();
        
        ArgumentNullException.ThrowIfNull(jobSettings);

        if (jobSettings.IsEnabled)
        {
            recurringJobManager.AddOrUpdate<TJob>(
                typeof(TJob).Name,
                v=>v.Process(CancellationToken.None),
                jobSettings.Cron);
        }
        
        return app;
    }
    
    private static IServiceCollection ConfigureDatabase(
        this IServiceCollection services,
        string connectionString,
        string migrationAssemblyName)
    {
        services.AddDbContext<HangfireDbContext>(optionsBuilder =>
        {
            optionsBuilder.EnableDetailedErrors();

            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }

}