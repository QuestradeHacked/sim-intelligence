using Domain.Utils;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace UnitTests.Domain.Utils;

public class CorrelationContextTests
{
    private readonly CorrelationContext _correlationContext;
    private readonly ILogger _logger;

    public CorrelationContextTests()
    {
        _logger = Substitute.For<ILogger>();
        _correlationContext = new CorrelationContext();
    }

    [Fact]
    public void AddLogScopeForSubscriber_ShouldSetCorrelationId_WhenHasOne()
    {
        // Arrange
        var correlationId = Guid.NewGuid().ToString();

        // Act
        _correlationContext.AddLogScopeForSubscriber(_logger, correlationId, "test");

        // Assert
        Assert.Equal(correlationId, _correlationContext.CorrelationId);
    }

    [Fact]
    public void AddLogScopeForSubscriber_ShouldUseADefaultCorrelationId_WhenDoesNotProvideOne()
    {
        // Act
        _correlationContext.AddLogScopeForSubscriber(_logger, null, "test");

        // Assert
        Assert.NotNull(_correlationContext.CorrelationId);
    }
}
