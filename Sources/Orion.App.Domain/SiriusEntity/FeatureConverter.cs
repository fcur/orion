using Venus.Shared.Domain;

namespace Orion.App.Domain.SiriusEntity;

public static class FeatureConverter
{
    public static Feature ToDomain(this FeatureState state, DateTimeOffset atTime)
    {
        return new(
            featureId: FeatureId.New,
            contentId: state.ContentId,
            contentTitle: state.ContentTitle,
            startAt: state.StartAt,
            endAt: state.EndAt,
            changedAt: atTime,
            contents: state.Contents,
            concurrencyToken: 0,
            version: DomainVersion.New);
    }
}


