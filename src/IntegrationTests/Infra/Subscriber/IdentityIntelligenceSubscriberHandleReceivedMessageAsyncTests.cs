using Domain.Services;
using Infra.Models.Messages;
using Infra.Services.Publisher;
using Infra.Services.Subscriber;
using Infra.Validators;
using IntegrationTests.Faker;
using IntegrationTests.Fixture;
using IntegrationTests.Providers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Questrade.Library.PubSubClientHelper.Primitives;
using Xunit;

namespace IntegrationTests.Infra.Subscriber;

public class
    IdentityIntelligenceSubscriberHandleReceivedMessageAsyncTests : IAssemblyFixture<PubSubEmulatorProcessFixture>
{
    private readonly MockLogger<IdentityIntelligenceSubscriberHandleReceivedMessageAsyncTests> _logger;

    private readonly IMediator _mediator;

    private readonly IMetricService _metricService;

    private readonly PubSubEmulatorProcessFixture _pubSubFixture;

    private readonly IPublisherHandlerService _publisherHandlerService;

    private readonly IdentityIntelligenceSubscriber _subscriber;

    private readonly int _subscriberTimeout;

    private readonly string _topicId;

    public IdentityIntelligenceSubscriberHandleReceivedMessageAsyncTests(PubSubEmulatorProcessFixture pubSubFixture)
    {
        _logger = new MockLogger<IdentityIntelligenceSubscriberHandleReceivedMessageAsyncTests>();
        Mock<ILoggerFactory> loggerFactory = new();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_logger);
        _mediator = Substitute.For<IMediator>();
        _metricService = Substitute.For<IMetricService>();
        _pubSubFixture = pubSubFixture;
        _publisherHandlerService = Substitute.For<IPublisherHandlerService>();
        _subscriberTimeout = _pubSubFixture.SubscriberTimeout;
        _topicId = $"T_{Guid.NewGuid()}";

        var services = new ServiceCollection();

        services.AddSingleton<IDefaultJsonSerializerOptionsProvider, MyDefaultJsonSerializerOptionsProvider>();
        var serviceProvider = services.AddMemoryCache().BuildServiceProvider();
        var subscriptionId = $"{_topicId}.{Guid.NewGuid()}";
        var subscriberConfig = _pubSubFixture.CreateDefaultSubscriberConfig(subscriptionId);
        var validator = new IdentityIntelligenceMessageValidator();

        _subscriber = new IdentityIntelligenceSubscriber(
            loggerFactory.Object,
            _mediator,
            _metricService,
            _publisherHandlerService,
            serviceProvider,
            subscriberConfig,
            validator
        );

        _pubSubFixture.CreateTopic(_topicId);
        _pubSubFixture.CreateSubscription(_topicId, subscriptionId);
    }

    [Theory]
    [MemberData(nameof(GetPossibleInvalidMessages))]
    public async Task HandleReceivedMessageAsync_ShouldLogWarning_WhenProfileIdCrmUserIdOrPhoneNumberIsEmptyOrNull(
        IdentityIntelligenceMessage identityIntelligenceMessage)
    {
        // Arrange
        var publisher = await _pubSubFixture.CreatePublisherAsync(_topicId);

        // Act
        await publisher.PublishAsync(JsonConvert.SerializeObject(identityIntelligenceMessage));
        await _subscriber.StartAsync(CancellationToken.None);
        await Task.Delay(_subscriberTimeout);
        await _subscriber.StopAsync(CancellationToken.None);

        var loggedMessages = _logger.GetAllMessages();

        // Assert
        Assert.Contains("A message with empty", loggedMessages);
        _metricService.Received(1).Increment(
            statName: Arg.Any<string>(),
            tags: Arg.Any<List<string>>()
        );
    }

    [Fact]
    public async Task HandleReceivedMessageAsync_ShouldLogError_WhenFailToHandleMessage()
    {
        // Arrange
        var publisher = await _pubSubFixture.CreatePublisherAsync(_topicId);
        var identityIntelligenceMessage = IdentityIntelligenceFaker.GetIdentityIntelligenceMessageFake();

        _mediator.Send(identityIntelligenceMessage, CancellationToken.None).ThrowsForAnyArgs(new Exception());

        // Act
        await publisher.PublishAsync(JsonConvert.SerializeObject(identityIntelligenceMessage));
        await _subscriber.StartAsync(CancellationToken.None);
        await Task.Delay(_subscriberTimeout);
        await _subscriber.StopAsync(CancellationToken.None);

        var loggedMessages = _logger.GetAllMessages();

        // Assert
        Assert.Contains(
            expectedSubstring: "Failed on handling the received message from",
            actualString: loggedMessages
        );
    }

    [Fact]
    public async Task HandleReceivedMessageAsync_ShouldCallMediator_WithIdentityIntelligenceMessage()
    {
        // Arrange
        var publisher = await _pubSubFixture.CreatePublisherAsync(_topicId);
        var identityIntelligenceMessage = IdentityIntelligenceFaker.GetIdentityIntelligenceMessageFake();

        _mediator.Send(identityIntelligenceMessage, CancellationToken.None)
            .ReturnsForAnyArgs(new IdentityIntelligenceResultMessage());

        // Act
        await publisher.PublishAsync(JsonConvert.SerializeObject(identityIntelligenceMessage));
        await _subscriber.StartAsync(CancellationToken.None);
        await Task.Delay(_subscriberTimeout);
        await _subscriber.StopAsync(CancellationToken.None);

        // Assert
        await _mediator.Received(1).Send(
            request: Arg.Any<IdentityIntelligenceMessage>(),
            cancellationToken: Arg.Any<CancellationToken>()
        );
        _metricService.Received(2).Increment(
            statName: Arg.Any<string>(), tags: Arg.Any<List<string>>()
        );
    }

    [Fact]
    public async Task HandleReceivedMessageAsync_ShouldCallPublisherService_WithScanResultFromProvider()
    {
        // Arrange
        var publisher = await _pubSubFixture.CreatePublisherAsync(_topicId);
        var identityIntelligenceMessage = IdentityIntelligenceFaker.GetIdentityIntelligenceMessageFake();
        var identityIntelligenceResultMessage = new IdentityIntelligenceResultMessage();

        _mediator.Send(identityIntelligenceMessage, CancellationToken.None)
            .ReturnsForAnyArgs(identityIntelligenceResultMessage);

        _publisherHandlerService.Publish(identityIntelligenceResultMessage, CancellationToken.None)
            .ReturnsForAnyArgs(Task.CompletedTask);

        // Act
        await publisher.PublishAsync(message: JsonConvert.SerializeObject(identityIntelligenceMessage));
        await _subscriber.StartAsync(CancellationToken.None);
        await Task.Delay(_subscriberTimeout);
        await _subscriber.StopAsync(CancellationToken.None);

        // Assert
        await _mediator.Received(1).Send(
            request: Arg.Any<IdentityIntelligenceMessage>(),
            cancellationToken: Arg.Any<CancellationToken>()
        );
        await _publisherHandlerService.Received(1).Publish(
            message: Arg.Any<IdentityIntelligenceResultMessage>(),
            cancellationToken: Arg.Any<CancellationToken>()
        );
        _metricService.Received(2).Increment(
            statName: Arg.Any<string>(),
            tags: Arg.Any<List<string>>()
        );
    }

    public static IEnumerable<object[]> GetPossibleInvalidMessages()
    {
        var faker = new Bogus.Faker();

        yield return new object[]
        {
            new IdentityIntelligenceMessage
            {
                Phone = faker.Phone.PhoneNumber("+###########"),
                ProfileId = null
            }
        };

        yield return new object[]
        {
            new IdentityIntelligenceMessage
            {
                Phone = faker.Phone.PhoneNumber("+###########"),
                CrmUserId = null
            }
        };

        yield return new object[]
        {
            new IdentityIntelligenceMessage
            {
                Phone = faker.Phone.PhoneNumber("+###########"),
                ProfileId = string.Empty
            }
        };

        yield return new object[]
        {
            new IdentityIntelligenceMessage
            {
                Phone = null!,
                ProfileId = faker.Random.Guid().ToString()
            }
        };
    }
}
