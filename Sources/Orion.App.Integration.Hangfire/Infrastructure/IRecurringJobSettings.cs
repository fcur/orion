namespace Orion.App.Integration.Hangfire.Infrastructure;

public class RecurringJobSettings
{
    public string Cron { get; set; } = null!;
    public bool IsEnabled { get; set; } = false;
}