using Venus.Shared.Domain;

namespace Orion.App.Domain.FeedEntity.Events;

public abstract class FeedBaseEvent: DomainEvent
{
    protected FeedBaseEvent(FeedId feedId, FeedName feedName,  DateTimeOffset startAt,  DomainVersion version, DateTimeOffset occuredAt) 
        : base(version, occuredAt)
    {
        FeedId = feedId;
        FeedName = feedName;
        StartAt = startAt;
    }
    
    public abstract string EventName { get; }
    
    public FeedId FeedId { get; }
    
    public  FeedName FeedName { get; }
    public  DateTimeOffset StartAt { get; }
}