namespace Orion.Integration.Kafka.IntegrationEvents;

public sealed class FeedCreationEvent
{
    public FeedCreationEvent(Guid id, string name, DateTimeOffset startAt, Dictionary<string, object> data, long version, DateTimeOffset eventTime)
    {
        Id = id;
        Name = name;
        StartAt = startAt;
        Data = data;
        Version = version;
        EventTime = eventTime;
    }
    
    public Guid Id { get; }
    
    public  string Name { get; }
    
    public  DateTimeOffset StartAt { get; }
    
    public Dictionary<string,object> Data { get; }
    
    public long Version { get; }
    
    public  DateTimeOffset EventTime { get; }
}