using FluentAssertions;
using Infra.Models.Messages;
using Xunit;

namespace UnitTests.Infra.Models.Messages;

public class IdentityIntelligenceScanResultMessageTests
{
    [Fact]
    public void IdentityIntelligenceResultMessage_ShouldNotReturnNullOrEmptyId_WhenIdentityIntelligenceResultMessageIsCreated()
    {
        // Act
        var result = new PubSubMessage<IdentityIntelligenceResultMessage>();

        // Assert
        result.Id.Should().NotBeNullOrEmpty();
    }
}
