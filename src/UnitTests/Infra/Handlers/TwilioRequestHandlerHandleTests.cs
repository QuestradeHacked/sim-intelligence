using Application.Handlers;
using Bogus;
using Domain.Services;
using FluentAssertions;
using Infra.Config.Twilio;
using Infra.Models.Messages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Twilio.Rest.Lookups.V2;
using Xunit;

namespace UnitTests.Infra.Handlers;

public class TwilioRequestHandlerHandleTests
{
    private readonly ILogger<TwilioRequestHandler> _logger;
    private readonly IMetricService _metricService;
    private readonly Faker<PhoneNumberResource> _faker = new();

    public TwilioRequestHandlerHandleTests()
    {
        _logger = Substitute.For<ILogger<TwilioRequestHandler>>();
        _metricService = Substitute.For<IMetricService>();
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenTwilioIsNotEnabled()
    {
        // Arrange
        var model = new IdentityIntelligenceMessage();
        var twilioConfiguration = new TwilioConfiguration { Enable = false, ApiKey = "TestApiKey", ApiKeySecret = "TestApiKeySecret", AccountSid = "TestSid", Fields = "caller_name,line_type_intelligence,sim_swap" };
        var twilioRequestHandler = Substitute.For<TwilioRequestHandler>(_logger, _metricService, twilioConfiguration);

        // Act
        var result = await twilioRequestHandler.Handle(model, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
