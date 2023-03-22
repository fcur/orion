namespace Orion.App.Domain.SiriusEntity;

public sealed record FeatureState(
    ContentId ContentId,
    ContentTitle ContentTitle,
    DateTimeOffset StartAt,
    DateTimeOffset? EndAt,
    FeatureContent[] Contents)
{
    public Dictionary<string, object> GetAvailableContent()
    {
        var data = Contents.Select(v => new AvailableContent(v.Type, v.Locale)).ToArray();
        return new Dictionary<string, object>() { { "Contents", data } };
    }
}