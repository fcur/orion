using CSharpFunctionalExtensions;
using Orion.App.Domain.FeedEntity.Events;
using Venus.Shared.Domain;

namespace Orion.App.Domain.FeedEntity;

public sealed class Feed: DomainEntity<FeedId>
{
    public Feed(FeedId feedId, FeedName feedName, DateTimeOffset startAt, IReadOnlyCollection<FeedData> data, uint concurrencyToken, DomainVersion version) 
        : base(feedId, version)
    {
        ArgumentNullException.ThrowIfNull(feedName);
        
        FeedName = feedName;
        StartAt = startAt;
        Data = data;
        ConcurrencyToken = concurrencyToken;
    }
    
    public  FeedName FeedName { get; private set; }
    
    public  DateTimeOffset StartAt { get; private set; }
    
    public  IReadOnlyCollection<FeedData> Data { get; private set; }
    
    public uint ConcurrencyToken { get; }

    public static Result<Feed, DomainError> Create(FeedId feedId, FeedName feedName, DateTimeOffset startAt, IReadOnlyCollection<FeedData> data, DateTimeOffset atTime)
    {
        if (startAt < atTime)
        {
            return FeedValidationError.InvalidStartTime;
        }

        var version = DomainVersion.New;
        var feed = new Feed(feedId, feedName, startAt, data, concurrencyToken:0, version: version);
        var creationEvent = new FeedCreationEvent(feedId, feedName, startAt, version, atTime);
        feed.AddEvent(creationEvent);
        return feed;
    }

    public Maybe<DomainError> Update(FeedName feedName, DateTimeOffset startAt, IReadOnlyCollection<FeedData> data, DateTimeOffset atTime)
    {
        if (startAt < atTime)
        {
            return FeedValidationError.InvalidStartTime;
        }
        
        FeedName = feedName;
        StartAt = startAt;
        Data = data;
        IncrementVersion();

        var updateEvent = new FeedUpdateEvent(Id, FeedName, StartAt, Version, atTime);
        AddEvent(updateEvent);
        
        return  Maybe<DomainError>.None;
    }
}