namespace Orion.App.Integration.DataProvider.Dto;

public sealed class FeedItemDto
{
    public string FeedId { get; set; } = null!;

    public string FeedName { get; set; } = null!;
    
    public  DateTimeOffset StartTime { get; set; }

    public FeedContentDto[] Content { get; set; } = Array.Empty<FeedContentDto>();
    
    public  long Timestamp { get; set; }
}