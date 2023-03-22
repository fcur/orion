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

public sealed class DataProviderTest
{
    [Theory, AutoData]
    public async Task LoadNewFeatures_ThenInsert(FeatureDto featureDto)
    {
        // Arrange
        var features = new ConcurrentBag<Feature>();
        var apiMock = CreateDataProviderApi(featureDto);
        var featureRepositoryMock = CreateFeatureInsertRepository(features);
        var featureQueryRepositoryMock = CreateFeatureQueryRepository(Array.Empty<Feature>());

        var job = new LoadDataJob(apiMock.Object, featureRepositoryMock.Object, featureQueryRepositoryMock.Object);

        // Act
        var feedsDto = await apiMock.Object.GetFeeds(requestParams: new GetEventsParams(), ct: default);
        await job.Process(default);


        // Assert
        using var scope = new AssertionScope();
        feedsDto.Single().Should().Be(featureDto);
        features.Count.Should().Be(1);
    }

    [Theory, AutoData]
    public async Task LoadChangedFeatures_ThenUpdate(FeatureDto changedFeatureDto, FeatureDto sameFeatureDto, FeatureState existingFeatureState)
    {
        // Arrange
        var atTime = DateTimeOffset.UtcNow;

        changedFeatureDto.Override(startTime: atTime.AddHours(2), endTime: atTime.AddHours(4), changedAt: atTime, name: "changed");
        sameFeatureDto.Override(startTime: atTime.AddMinutes(102), endTime: null, changedAt: atTime, name: "same");

        var features = new ConcurrentBag<Feature>();
        var apiMock = CreateDataProviderApi(changedFeatureDto, sameFeatureDto);
        var featureRepositoryMock = CreateFeatureUpdateRepository(features);

        var stateBeforeUpdate = existingFeatureState with
        {
            ContentId = new ContentId(changedFeatureDto.Id),
            StartAt = atTime.AddHours(1),
            EndAt = null
        };

        var featureQueryRepositoryMock = CreateFeatureQueryRepository(
            stateBeforeUpdate.ToDomain(atTime),
            sameFeatureDto.ToState().ToDomain(atTime));

        var job = new LoadDataJob(apiMock.Object, featureRepositoryMock.Object, featureQueryRepositoryMock.Object);

        // Act
        var feedsDto = await apiMock.Object.GetFeeds(requestParams: new GetEventsParams(), ct: default);
        await job.Process(default);

        // Assert
        using var scope = new AssertionScope();
        feedsDto.Should().Contain(new[] { changedFeatureDto, sameFeatureDto });
        features.Should().ContainSingle(v
            => v.ContentId == stateBeforeUpdate.ContentId
            && v.ContentTitle.Value == changedFeatureDto.Name
            && v.StartAt == changedFeatureDto.StartTime
            && v.EndAt == changedFeatureDto.EndTime);
    }

    private static Mock<IDataProviderApi> CreateDataProviderApi(params FeatureDto[] features)
    {
        var mock = new Mock<IDataProviderApi>();
        {
            mock.Setup(v => v.GetFeeds(It.IsAny<GetEventsParams>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetEventsParams requestParams, CancellationToken ct) => { return features; });
        }

        return mock;
    }

    private static Mock<IFeatureRepository> CreateFeatureInsertRepository(ConcurrentBag<Feature> features)
    {
        var mock = new Mock<IFeatureRepository>();
        {
            mock
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

        return mock;
    }
    private static Mock<IFeatureRepository> CreateFeatureUpdateRepository(ConcurrentBag<Feature> features)
    {
        var mock = new Mock<IFeatureRepository>();
        {
            mock
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

        return mock;
    }


    private static Mock<IFeatureQueryRepository> CreateFeatureQueryRepository(params Feature[] features)
    {
        var mock = new Mock<IFeatureQueryRepository>();
        {
            mock
               .Setup(v => v.Find(It.IsAny<IReadOnlyCollection<ContentId>>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((IReadOnlyCollection<ContentId> contentIds, CancellationToken ct) => features);
        }

        return mock;
    }
}