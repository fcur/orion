namespace Orion.App.Integration.Hangfire.Infrastructure;

public interface IHangfireJob
{
    Task Process(CancellationToken cancellationToken);
}