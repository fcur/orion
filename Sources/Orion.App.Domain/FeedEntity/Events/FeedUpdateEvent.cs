using Venus.Shared.Domain;

namespace Orion.App.Domain.FeedEntity.Events;

public sealed class FeedUpdateEvent: FeedBaseEvent
{
    public FeedUpdateEvent(FeedId feedId, FeedName feedName, DateTimeOffset startAt, DomainVersion version, DateTimeOffset occuredAt) 
        : base(feedId, feedName, startAt, version, occuredAt)
    {
        
    }

    public override string EventName => "Update";
}