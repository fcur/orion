using System.Collections.Concurrent;
using Moq;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Orion.App.Domain.SiriusEntity;
using Orion.App.Integration.Hangfire.Jobs;
using Orion.App.Integration.SiriusProvider.Converters;
using Orion.App.Integration.SiriusProvider.Dto;

namespace Orion.App.Integration.SiriusProvider.Tests;

public class DataProviderTest
{
    [Theory, AutoData]
    public async Task LoadNewFeatures_ThenInsert(FeatureDto featureDto)
    {
        // Arrange
        var api = new Mock<IDataProviderApi>();
        {
            api.Setup(v => v.GetFeeds(It.IsAny<GetEventsParams>()))
                .ReturnsAsync(() => { return new[] { featureDto }; });
        }

        var features = new ConcurrentBag<Feature>();
        var featureRepository = new Mock<IFeatureRepository>();
        {
            featureRepository
                .Setup(v => v.InsertRange(It.IsAny<IReadOnlyCollection<Feature>>(), It.IsAny<CancellationToken>()))
                .Returns((IReadOnlyCollection<Feature> newFeatures, CancellationToken ct) =>
                {
                    foreach (var item in newFeatures)
                    {
                        features.Add(item);
                    }

                    return Task.CompletedTask;
                });
        }

        var featureQueryRepository = new Mock<IFeatureQueryRepository>();
        {
            featureQueryRepository
                .Setup(v => v.Find(It.IsAny<IReadOnlyCollection<ContentId>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IReadOnlyCollection<ContentId> contentIds, CancellationToken ct) =>
                    Array.Empty<Feature>());
        }

        var job = new LoadDataJob(api.Object, featureRepository.Object, featureQueryRepository.Object);

        // Act
        await job.Process(default);

        // Assert
        using var scope = new AssertionScope();
        features.Count.Should().Be(1);
    }

    [Theory, AutoData]
    public async Task LoadChangedFeatures_ThenUpdate(FeatureDto updatedFeatureDto, FeatureDto sameFeatureDto, FeatureState featureState)
    {
        // Arrange
        var atTime = DateTimeOffset.UtcNow;
        var existingStartAt = atTime.AddMinutes(12);
        var updatedStartAt = atTime.AddHours(2);
        updatedFeatureDto.Override(startTime: updatedStartAt, endTime: null, changedAt: atTime);
        sameFeatureDto.Override(startTime: existingStartAt, endTime: null, changedAt: atTime, name: "same");
        
        var api = new Mock<IDataProviderApi>();
        {
            api.Setup(v => v.GetFeeds(It.IsAny<GetEventsParams>()))
                .ReturnsAsync(() => new[]
                {
                    updatedFeatureDto, sameFeatureDto
                });
        }

        var features = new ConcurrentBag<Feature>();
        var featureRepository = new Mock<IFeatureRepository>();
        {
            featureRepository
                .Setup(v => v.UpdateRange(It.IsAny<IReadOnlyCollection<Feature>>(), It.IsAny<CancellationToken>()))
                .Returns((IReadOnlyCollection<Feature> newFeatures, CancellationToken ct) =>
                {
                    foreach (var item in newFeatures)
                    {
                        features.Add(item);
                    }
                    return Task.CompletedTask;
                });
        }

        var updatedContentId = new ContentId(updatedFeatureDto.Id);
        var updatedFeatureState = featureState with { ContentId = updatedContentId, StartAt = existingStartAt, EndAt = null};
        var updatedFeature = Feature.Create(updatedFeatureState, atTime).Value;

        var sameFeatureState = sameFeatureDto.ToState();
        var sameFeature = Feature.Create(sameFeatureState, atTime).Value;
        
        var featureQueryRepository = new Mock<IFeatureQueryRepository>();
        {
            featureQueryRepository
                .Setup(v => v.Find(It.IsAny<IReadOnlyCollection<ContentId>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IReadOnlyCollection<ContentId> contentIds, CancellationToken ct) => new[]
                {
                    updatedFeature,
                    sameFeature
                });
        }

        var job = new LoadDataJob(api.Object, featureRepository.Object, featureQueryRepository.Object);

        // Act
        await job.Process(default);

        // Assert
        using var scope = new AssertionScope();
        features.Single().ContentId.Should().Be(updatedContentId);
    }
}