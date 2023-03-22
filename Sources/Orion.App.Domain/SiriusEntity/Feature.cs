using CSharpFunctionalExtensions;
using Orion.App.Domain.SiriusEntity.DomainEvents;
using Orion.App.Domain.SiriusEntity.Events;
using Venus.Shared.Domain;

namespace Orion.App.Domain.SiriusEntity;

public sealed class Feature : DomainEntity<FeatureId>
{
    public Feature(
        FeatureId featureId,
        ContentId contentId,
        ContentTitle contentTitle,
        DateTimeOffset startAt,
        DateTimeOffset? endAt,
        DateTimeOffset changedAt,
        IReadOnlyCollection<FeatureContent> contents,
        uint concurrencyToken,
        DomainVersion version)
        : base(featureId, version)
    {
        ArgumentNullException.ThrowIfNull(contentId);
        ArgumentNullException.ThrowIfNull(contentTitle);
        ArgumentNullException.ThrowIfNull(contents);

        ContentId = contentId;
        ContentTitle = contentTitle;
        StartAt = startAt;
        EndAt = endAt;
        ChangedAt = changedAt;
        Contents = contents;
        ConcurrencyToken = concurrencyToken;
    }

    public ContentId ContentId { get; }
    public ContentTitle ContentTitle { get; private set; }
    public DateTimeOffset StartAt { get; private set; }
    public DateTimeOffset? EndAt { get; private set; }
    public DateTimeOffset ChangedAt { get; private set; }
    public IReadOnlyCollection<FeatureContent> Contents { get; private set; }
    public uint ConcurrencyToken { get; }

    public FeatureState State => new(ContentId, ContentTitle, StartAt, EndAt, Contents.ToArray());

    public static Result<Feature, DomainError> Create(FeatureState state, DateTimeOffset atTime)
    {
        if (state.StartAt < atTime)
        {
            return FeatureValidationError.InvalidStartTime;
        }

        var featureId = FeatureId.New;
        var version = DomainVersion.New;
        var feature = state.ToDomain(atTime);

        var availableContent = state.GetAvailableContent();
        var creationEvent = new SiriusContentCreationEvent(featureId, state.ContentTitle, state.StartAt,
            availableContent, version, atTime);
        feature.AddEvent(creationEvent);

        return feature;
    }

    public Maybe<DomainError> MaybeUpdate(FeatureState newState, DateTimeOffset atTime)
    {
        if (newState.StartAt < atTime)
        {
            return FeatureValidationError.InvalidStartTime;
        }

        if (newState == State)
        {
            return FeatureValidationError.NoChanges;
        }

        ContentTitle = newState.ContentTitle;
        StartAt = newState.StartAt;
        EndAt = newState.EndAt;
        Contents = newState.Contents;
        ChangedAt = atTime;
        IncrementVersion();

        var availableContent = newState.GetAvailableContent();
        var updateEvent = new SiriusContentUpdateEvent(Id, ContentTitle, StartAt, availableContent, Version, atTime);
        AddEvent(updateEvent);

        return Maybe<DomainError>.None;
    }
}