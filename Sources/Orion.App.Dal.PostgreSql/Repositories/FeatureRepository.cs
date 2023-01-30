using Microsoft.EntityFrameworkCore;
using Orion.App.Dal.PostgreSql.Converters;
using Orion.App.Domain.SiriusEntity;
using Orion.App.Domain.SiriusEntity.DomainEvents;
using Orion.App.Domain.SiriusEntity.Events;
using Venus.Shared.Domain;

namespace Orion.App.Dal.PostgreSql.Repositories;

public sealed class FeatureRepository: IFeatureRepository
{
    private readonly OrionDbContext _dbContext;
    private readonly DomainEventPublisher _domainEventPublisher;

    public FeatureRepository(OrionDbContext dbContext, DomainEventPublisher domainEventPublisher)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(domainEventPublisher);
        _dbContext = dbContext;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<Feature?> Find(FeatureId featureId, CancellationToken cancellationToken)
    {
        var dal = await _dbContext.Features
            .AsNoTracking()
            .SingleOrDefaultAsync(v => v.Id == featureId.Value, cancellationToken);
        
        return dal?.ToDomain();
    }

    public async Task InsertRange(IReadOnlyCollection<Feature> feeds, CancellationToken cancellationToken)
    {
        if (!feeds.Any())
        {
            return;
        }
        
        var dals = feeds.Select(v => v.ToDal()).ToArray();
       _dbContext.Features.AddRange(dals);

       await _dbContext.SaveChangesAsync(cancellationToken);
       await PublishDomainEvents(feeds, cancellationToken);
    }

    public async Task UpdateRange(IReadOnlyCollection<Feature> feeds, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var feed in feeds)
            {
                var feedDal = feed.ToDal();
                var existingFeed = _dbContext.Features.FirstOrDefault(v => v.Id == feedDal.Id);
            
                if(existingFeed is null)
                {
                    _dbContext.Features.Update(feedDal);
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

    private async Task PublishDomainEvents(IReadOnlyCollection<Feature> feeds, CancellationToken cancellationToken)
    {
        var domainEvents = feeds
            .SelectMany(v => v.GrabEvents())
            .Cast<SiriusContentBaseEvent>()
            .ToArray();
        
        await _domainEventPublisher.Publish(domainEvents, cancellationToken);
    }
}