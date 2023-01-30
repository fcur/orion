using System.Text.Json.Serialization;

namespace Orion.App.Integration.SiriusProvider.Dto;

public sealed class FeatureDto
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("title")] public string Name { get; set; } = null!;
    [JsonPropertyName("category")] public string Category { get; set; } = null!;
    [JsonPropertyName("startTime")] public DateTimeOffset StartTime { get; set; }
    [JsonPropertyName("endTime")] public DateTimeOffset? EndTime { get; set; }
    [JsonPropertyName("changedAt")] public DateTimeOffset ChangedAt { get; set; }
    [JsonPropertyName("timestamp")] public long Timestamp { get; set; }
    [JsonPropertyName("contents")] public EventContentDto[] Contents { get; set; } = Array.Empty<EventContentDto>();
}