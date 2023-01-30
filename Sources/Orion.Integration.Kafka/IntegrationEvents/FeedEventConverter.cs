using Orion.App.Domain.SiriusEntity.DomainEvents;

namespace Orion.Integration.Kafka.IntegrationEvents;

public static class FeedEventConverter
{
    public static FeedCreationEvent ToIntegrationEvent(this SiriusContentCreationEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        
        var data = new Dictionary<string, object>()
        {
            { "Data", "TBD" }
        };

        return new(
            domainEvent.FeatureId.Value, 
            domainEvent.ContentTitle.Value, 
            domainEvent.StartAt,data, 
            domainEvent.Version.Value, 
            domainEvent.OccuredAt);
    }
}