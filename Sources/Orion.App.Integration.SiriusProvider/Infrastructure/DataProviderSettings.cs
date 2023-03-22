namespace Orion.App.Integration.SiriusProvider.Infrastructure;

public sealed class DataProviderSettings
{
    public Uri BaseUri { get; set; } = null!;
    
    public  TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
}