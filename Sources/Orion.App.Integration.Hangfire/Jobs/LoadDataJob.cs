using Orion.App.Domain.FeedEntity;
using Orion.App.Integration.DataProvider;
using Orion.App.Integration.Hangfire.Infrastructure;

namespace Orion.App.Integration.Hangfire.Jobs;

public sealed class LoadDataJob: IHangfireJob
{
    public const string SettingsName = "LoadDataJobSettings";

    private  readonly IDataProviderApi _dataProviderApi;
    private readonly IFeedRepository _feedRepository;
    
    public LoadDataJob(IDataProviderApi dataProviderApi, IFeedRepository feedRepository)
    {
        ArgumentNullException.ThrowIfNull(dataProviderApi);
        ArgumentNullException.ThrowIfNull(feedRepository);
        
        _dataProviderApi = dataProviderApi;
        _feedRepository = feedRepository;
    }
    
    public Task Process(CancellationToken cancellationToken)
    {
        // load data
        // insert new
        // update existing
        // success
        // try again later
        throw new NotImplementedException();
    }
}