using Orion.App.Dal.PostgreSql.Entities;
using Orion.App.Domain.SiriusEntity;
using Venus.Shared.Domain;

namespace Orion.App.Dal.PostgreSql.Converters;

internal static class FeatureDalConverter
{
    public static FeatureDal ToDal(this Feature domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        var data = domain.Contents.Select(v => v.ToDal()).ToArray();
        
        return new FeatureDal()
        {
            Id = domain.Id.Value,
            ContentId = domain.ContentId.Value,
            Title = domain.ContentTitle.Value,
            StartAt = domain.StartAt,
            EndAt = domain.EndAt,
            ChangedAt = domain.ChangedAt,
            Contents = data,
            Version = domain.Version.Value,
            ConcurrencyToken = domain.ConcurrencyToken
        };
    }

    public static Feature ToDomain(this FeatureDal dal)
    {
        ArgumentNullException.ThrowIfNull(dal);

        var contentId = new ContentId(dal.ContentId);
        var featureId = new FeatureId(dal.Id);
        var featureTitle = new ContentTitle(dal.Title);
        var data = dal.Contents.Select(v => v.ToDomain()).ToArray();
        var version = new DomainVersion(dal.Version);

        return new Feature(featureId, contentId, featureTitle, dal.StartAt, dal.EndAt,dal.ChangedAt, data, dal.ConcurrencyToken, version);
    }

    private static FeatureContentDal ToDal(this FeatureContent domain)
    {
        return new FeatureContentDal()
        {
            Id = domain.Id,
            Locale = domain.Locale,
            Type = domain.Type
        };
    }

    private static FeatureContent ToDomain(this FeatureContentDal dal)
    {
        return new FeatureContent(dal.Id, dal.Locale, dal.Type);
    }
}