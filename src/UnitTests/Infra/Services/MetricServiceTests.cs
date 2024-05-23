using Domain.Constants;
using Infra.Services;
using NSubstitute;
using StatsdClient;
using Xunit;

namespace UnitTests.Infra.Services;

public class MetricServiceTests
{
    private readonly IDogStatsd _dogStatsd;
    private readonly MetricService _service;

    public MetricServiceTests()
    {
        _dogStatsd = Substitute.For<IDogStatsd>();
        _service = new MetricService(_dogStatsd);
    }

    [Fact]
    public void Distribution_ShouldCallDogStatsdClient()
    {
        // Arrange
        const double latency = 101;

        // Act
        _service.Distribution(
            statName: MetricNames.IdentityIntelligenceHandleMessageCount,
            latency,
            tags: new List<string>()
        );

        // Assert
        _dogStatsd.Received(1).Distribution(
            statName: Arg.Is(MetricNames.IdentityIntelligenceHandleMessageCount),
            value: Arg.Is(latency),
            tags: Arg.Any<string[]>()
        );
    }

    [Fact]
    public void Increment_ShouldCallDogStatsdClient()
    {
        // Act
        _service.Increment(
            statName: MetricNames.IdentityIntelligenceHandleMessageCount,
            tags: new List<string>()
        );

        // Assert
        _dogStatsd.Received(1).Increment(
            statName: Arg.Is(MetricNames.IdentityIntelligenceHandleMessageCount),
            tags: Arg.Any<string[]>()
        );
    }

    [Fact]
    public void StartTimer_ShouldCallDogStatsdClient()
    {
        // Act
        _service.StartTimer(
            statName: MetricNames.IdentityIntelligenceHandleMessageCount,
            tags: new List<string>()
        );

        // Assert
        _dogStatsd.Received(1).StartTimer(
            name: Arg.Is(MetricNames.IdentityIntelligenceHandleMessageCount),
            tags: Arg.Any<string[]>()
        );
    }
}
