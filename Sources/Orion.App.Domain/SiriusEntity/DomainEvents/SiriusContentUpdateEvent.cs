using Orion.App.Domain.SiriusEntity.DomainEvents;
using Venus.Shared.Domain;

namespace Orion.App.Domain.SiriusEntity.Events;

public sealed class SiriusContentUpdateEvent: SiriusContentBaseEvent
{
    public SiriusContentUpdateEvent(
        FeatureId featureId, ContentTitle contentTitle, 
        DateTimeOffset startAt, Dictionary<string, object> data, 
        DomainVersion version, DateTimeOffset occuredAt) 
        : base(featureId, contentTitle, startAt, version, occuredAt)
    {
        Data = data;
    }

    public IReadOnlyDictionary<string, object> Data { get; }

    public override string EventName => "Update";
}