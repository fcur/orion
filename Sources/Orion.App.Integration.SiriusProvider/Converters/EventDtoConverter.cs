using System.Runtime.InteropServices.JavaScript;
using Orion.App.Domain.SiriusEntity;
using Orion.App.Integration.SiriusProvider.Dto;
using Venus.Shared.Domain;

namespace Orion.App.Integration.SiriusProvider.Converters;

public static class FeatureItemDtoConverter
{
    private const string FalseContentStatus = "br:vcm:false";

    public static FeatureState ToState(this FeatureDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var contentId = new ContentId(dto.Id);
        var featureTitle = new ContentTitle(dto.Name);
        var data = dto.Contents.Select(v => v.ToDomain()).Where(v => v != null).ToArray();

        return new FeatureState(contentId, featureTitle,
            dto.StartTime, dto.EndTime, data!);
    }

    public static FeatureDto Override(this FeatureDto dto, string? id = null, string? name = null,
        string? category = null, DateTimeOffset? startTime = null, DateTimeOffset? endTime = null,
        DateTimeOffset? changedAt = null, long? timestamp = null, EventContentDto[]? contents = null)
    {
        dto.Id = id ?? dto.Id;
        dto.Name = name ?? dto.Name;
        dto.Category = category ?? dto.Category;
        dto.StartTime = startTime ?? dto.StartTime;
        dto.EndTime = endTime ?? dto.EndTime;
        dto.ChangedAt = changedAt ?? dto.ChangedAt;
        dto.Timestamp = timestamp ?? dto.Timestamp;
        dto.Contents = contents ?? dto.Contents;

        return dto;
    }

    private static FeatureContent? ToDomain(this EventContentDto dto)
    {
        return dto.Status.StartsWith(FalseContentStatus)
            ? default
            : new FeatureContent(dto.Id, dto.Locale, dto.ContentType);
    }
}