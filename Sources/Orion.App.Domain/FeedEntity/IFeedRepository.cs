namespace Orion.App.Domain.FeedEntity;

public interface IFeedRepository
{
    Task<Feed?> Find(FeedId feedId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Feed>> Get(IReadOnlyCollection<FeedId> feedIds, CancellationToken cancellationToken);
    
    Task InsertRange(IReadOnlyCollection<Feed> feeds, CancellationToken cancellationToken);

    Task UpdateRange(IReadOnlyCollection<Feed> feeds, CancellationToken cancellationToken);
}