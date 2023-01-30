using Venus.Shared.Domain;

namespace Orion.App.Domain.SiriusEntity.DomainEvents;

public abstract class SiriusContentBaseEvent: DomainEvent
{
    protected SiriusContentBaseEvent(FeatureId featureId, ContentTitle contentTitle,  DateTimeOffset startAt,  DomainVersion version, DateTimeOffset occuredAt) 
        : base(version, occuredAt)
    {
        FeatureId = featureId;
        ContentTitle = contentTitle;
        StartAt = startAt;
    }
    
    public abstract string EventName { get; }
    
    public FeatureId FeatureId { get; }
    
    public  ContentTitle ContentTitle { get; }
    public  DateTimeOffset StartAt { get; }
}