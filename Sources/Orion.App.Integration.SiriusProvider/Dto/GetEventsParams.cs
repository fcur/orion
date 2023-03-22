using Refit;

namespace Orion.App.Integration.SiriusProvider.Dto;

public class GetEventsParams
{
    [AliasAs("auth")] public string Auth { get; set; } = null!;

    [AliasAs("userId")] public string UserId { get; set; } = null!;

    [Query] [AliasAs("page")] public uint Page { get; set; }

    [Query] [AliasAs("size")] public uint Size { get; set; }

    [Query] [AliasAs("timestamp")] public long TimeStamp { get; set; }
}