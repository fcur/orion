namespace Orion.Integration.Kafka.IntegrationEvents;

public static class FeedEventConverter
{
    public static FeedCreationEvent ToIntegrationEvent(this App.Domain.FeedEntity.Events.FeedCreationEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        
        var data = new Dictionary<string, object>()
        {
            { "Data", "TBD" }
        };

        return new(
            domainEvent.FeedId.Id, 
            domainEvent.FeedName.Value, 
            domainEvent.StartAt,data, 
            domainEvent.Version.Value, 
            domainEvent.OccuredAt);
    }
}