namespace Orion.App.Domain.SiriusEntity;

public interface IFeatureRepository
{
    Task<Feature?> Find(FeatureId featureId, CancellationToken cancellationToken);
 
    Task InsertRange(IReadOnlyCollection<Feature> feeds, CancellationToken cancellationToken);

    Task UpdateRange(IReadOnlyCollection<Feature> feeds, CancellationToken cancellationToken);
}