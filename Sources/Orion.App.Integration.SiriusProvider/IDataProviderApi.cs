using Orion.App.Integration.SiriusProvider.Dto;
using Refit;

namespace Orion.App.Integration.SiriusProvider;

public interface IDataProviderApi
{
    [Get("api/events")]
    Task<FeatureDto[]> GetFeeds(GetEventsParams requestParams, CancellationToken ct);
}