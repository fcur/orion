using FluentAssertions;
using FluentAssertions.Execution;
using Orion.App.Domain.SiriusEntity;

namespace Orion.App.Domain.UnitTests;

public class FeedTests
{
    // [Fact]
    // public void Creation_Test_Then_Success()
    // {
    //     // Arrange
    //     var id = new FeatureId(Guid.NewGuid());
    //     var originId = new ContentId("provider_feedId");
    //     var feedName = new ContentTitle("event_name");
    //     var now = DateTimeOffset.UtcNow;
    //     var startTime = now.AddDays(12);
    //     var feedInnerData = new[] { new FeedData("ax", "bx", "cx", Dx: null) };
    //     // Act
    //     var feedCreationResult = Feature.Create(id,originId, feedName, startTime, feedInnerData, now);
    //     
    //     // Assert
    //     using var scope = new AssertionScope();
    //     feedCreationResult.IsSuccess.Should().BeTrue();
    //     feedCreationResult.Value.Should().NotBeNull();
    //     feedCreationResult.Value.Id.Should().Be(id);
    // }
}