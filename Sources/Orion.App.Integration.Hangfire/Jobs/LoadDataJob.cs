using Orion.App.Domain.SiriusEntity;
using Orion.App.Integration.SiriusProvider;
using Orion.App.Integration.SiriusProvider.Dto;
using Orion.App.Integration.Hangfire.Infrastructure;
using Orion.App.Integration.SiriusProvider.Converters;

namespace Orion.App.Integration.Hangfire.Jobs;

public sealed class LoadDataJob : IHangfireJob
{
    public const string SettingsName = "LoadDataJobSettings";
    private readonly IDataProviderApi _dataProviderApi;
    private readonly IFeatureRepository _featureRepository;
    private readonly IFeatureQueryRepository _featureQueryRepository;

    public LoadDataJob(IDataProviderApi dataProviderApi, 
        IFeatureRepository featureRepository,
        IFeatureQueryRepository featureQueryRepository)
    {
        ArgumentNullException.ThrowIfNull(dataProviderApi);
        ArgumentNullException.ThrowIfNull(featureRepository);
        ArgumentNullException.ThrowIfNull(featureQueryRepository);

        _dataProviderApi = dataProviderApi;
        _featureRepository = featureRepository;
        _featureQueryRepository = featureQueryRepository;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var atTime = DateTimeOffset.UtcNow;
        var requestParams = new GetEventsParams();

        var latestDataStates = await LoadProviderData(requestParams, cancellationToken);
        var existingFeatures = await FindFeatures(latestDataStates, cancellationToken);

        var existingFeatureContentIds = existingFeatures.Select(v => v.ContentId);
        var newFeatureStates = latestDataStates.ExceptBy(existingFeatureContentIds, v => v.ContentId).ToArray();
        await InsertNewData(newFeatureStates, atTime, cancellationToken);

        var maybeChangedFeatureStates = latestDataStates.Except(newFeatureStates).ToArray();
        await UpdateExistingData(maybeChangedFeatureStates, existingFeatures, atTime, cancellationToken);
    }

    private async Task<IReadOnlyCollection<FeatureState>> LoadProviderData(GetEventsParams requestParams,
        CancellationToken cancellationToken)
    {
        var latestData = await _dataProviderApi.GetFeeds(requestParams);
        return latestData.Select(v => v.ToState()).ToArray();
    }

    private Task<IReadOnlyCollection<Feature>> FindFeatures(IReadOnlyCollection<FeatureState> featureStates,
        CancellationToken cancellationToken)
    {
        var originIds = featureStates.Select(v => v.ContentId).ToArray();
        return _featureQueryRepository.Find(originIds, cancellationToken);
    }

    private Task InsertNewData(IReadOnlyCollection<FeatureState> stateItems, DateTimeOffset atTime,
        CancellationToken cancellationToken)
    {
        if (!stateItems.Any())
        {
            return Task.CompletedTask;
        }

        var newFeedsResults = stateItems.Select(v => Feature.Create(v, atTime)).ToArray();
        var newFeatures = newFeedsResults.Where(v => v.IsSuccess)
            .Select(v => v.Value).ToArray();
        return _featureRepository.InsertRange(newFeatures, cancellationToken);
    }

    private Task UpdateExistingData(IReadOnlyCollection<FeatureState> stateItems,
        IReadOnlyCollection<Feature> featureItems, DateTimeOffset atTime, CancellationToken cancellationToken)
    {
        var stateItemsDictionary = stateItems.GroupBy(v => v.ContentId).ToDictionary(v => v.Key, v => v.First());

        var changedFeatures = new List<Feature>(featureItems.Count);
        foreach (var item in featureItems)
        {
            if (!stateItemsDictionary.TryGetValue(item.ContentId, out var state) 
                || item.Update(state!, atTime).HasValue)
            {
                continue;
            }

            changedFeatures.Add(item);
        }
        
        return _featureRepository.UpdateRange(changedFeatures, cancellationToken);
    }
}