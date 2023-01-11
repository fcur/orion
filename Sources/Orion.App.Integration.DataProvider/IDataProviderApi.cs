using Orion.App.Integration.DataProvider.Dto;
using Refit;

namespace Orion.App.Integration.DataProvider;

public interface IDataProviderApi
{
    [Get("feed/events")]
    Task<FeedItemDto[]> GetFeeds(GetFeedsParams requestParams);
}