using Microsoft.EntityFrameworkCore;
using Orion.App.Dal.PostgreSql.Converters;
using Orion.App.Domain.FeedEntity;
using Orion.App.Domain.FeedEntity.Events;
using Venus.Shared.Domain;

namespace Orion.App.Dal.PostgreSql.Repositories;

public sealed class FeedRepository: IFeedRepository
{
    private readonly OrionDbContext _dbContext;
    private readonly DomainEventPublisher _domainEventPublisher;

    public FeedRepository(OrionDbContext dbContext, DomainEventPublisher domainEventPublisher)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(domainEventPublisher);
        _dbContext = dbContext;
        _domainEventPublisher = domainEventPublisher;
    }
    
    public async Task<Feed?> Find(FeedId feedId, CancellationToken cancellationToken)
    {
        var dal = await _dbContext.Feeds
            .AsNoTracking()
            .SingleOrDefaultAsync(v => v.Id == feedId.Id, cancellationToken);
        
        return dal?.ToDomain();
    }

    public async Task<IReadOnlyCollection<Feed>> Get(IReadOnlyCollection<FeedId> feedIds, CancellationToken cancellationToken)
    {
        var feedIdsDal = feedIds.Select(v => v.Id).ToArray();

        var dals = await _dbContext.Feeds
            .AsNoTracking()
            .Where(v => feedIdsDal.Contains(v.Id))
            .ToArrayAsync(cancellationToken);

        return dals.Select(v => v.ToDomain()).ToArray();
    }

    public async Task InsertRange(IReadOnlyCollection<Feed> feeds, CancellationToken cancellationToken)
    {
        if (!feeds.Any())
        {
            return;
        }
        
        var dals = feeds.Select(v => v.ToDal()).ToArray();
       _dbContext.Feeds.AddRange(dals);

       await _dbContext.SaveChangesAsync(cancellationToken);
       await PublishDomainEvents(feeds, cancellationToken);
    }

    public async Task UpdateRange(IReadOnlyCollection<Feed> feeds, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var feed in feeds)
            {
                var feedDal = feed.ToDal();
                var existingFeed = _dbContext.Feeds.FirstOrDefault(v => v.Id == feedDal.Id);
            
                if(existingFeed is null)
                {
                    _dbContext.Feeds.Update(feedDal);
                }
                else
                {
                    var feedEntry = _dbContext.Entry(existingFeed);
                    feedEntry.CurrentValues.SetValues(feedDal);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await PublishDomainEvents(feeds, cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // todo: add exception handling
            throw;
        }
    }

    private async Task PublishDomainEvents(IReadOnlyCollection<Feed> feeds, CancellationToken cancellationToken)
    {
        var domainEvents = feeds
            .SelectMany(v => v.GrabEvents())
            .Cast<FeedBaseEvent>()
            .ToArray();
        
        await _domainEventPublisher.Publish(domainEvents, cancellationToken);
    }
}