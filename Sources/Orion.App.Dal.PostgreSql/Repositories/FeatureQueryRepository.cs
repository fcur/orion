using Microsoft.EntityFrameworkCore;
using Orion.App.Dal.PostgreSql.Converters;
using Orion.App.Domain.SiriusEntity;

namespace Orion.App.Dal.PostgreSql.Repositories;

public sealed class FeatureQueryRepository : IFeatureQueryRepository
{
    private readonly OrionDbContext _dbContext;

    public FeatureQueryRepository(OrionDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyCollection<Feature>> Find(IReadOnlyCollection<ContentId> contentIds,
        CancellationToken cancellationToken)
    {
        if (contentIds.Any())
        {
            return Array.Empty<Feature>();
        }

        var originIdsDal = contentIds.Select(v => v.Value).ToArray();

        var dals = await _dbContext.Features
            .AsNoTracking()
            .Where(v => originIdsDal.Contains(v.ContentId))
            .ToArrayAsync(cancellationToken);

        return dals.Select(v => v.ToDomain()).ToArray();
    }
}