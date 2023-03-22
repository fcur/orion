namespace Orion.App.Domain.SiriusEntity;

public interface IFeatureQueryRepository
{
    Task<IReadOnlyCollection<Feature>> Find(IReadOnlyCollection<ContentId> contentIds, CancellationToken cancellationToken);
}