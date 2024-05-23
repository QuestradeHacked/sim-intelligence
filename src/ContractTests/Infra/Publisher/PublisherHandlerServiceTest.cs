using ContractTests.Fixture;
using ContractTests.Helpers;
using ContractTests.Infra.Publisher.Contracts;
using ContractTests.Providers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Questrade.Library.PubSubClientHelper.Primitives;
using Xunit;

namespace ContractTests.Infra.Publisher;

public class PublisherHandlerServiceTest : IAssemblyFixture<PubSubEmulatorProcessFixture>
{
    private readonly PubSubEmulatorProcessFixture _pubSubFixture;

    private readonly string _topicId;

    private readonly string _subscriptionId;

    public PublisherHandlerServiceTest(PubSubEmulatorProcessFixture pubSubFixture)
    {
        _pubSubFixture = pubSubFixture;
        _topicId = $"T_{Guid.NewGuid()}";
        _subscriptionId = $"{_topicId}.{Guid.NewGuid()}";

        var services = new ServiceCollection();

        services.AddSingleton<IDefaultJsonSerializerOptionsProvider, MyDefaultJsonSerializerOptionsProvider>();

        services.BuildServiceProvider();

        _pubSubFixture.CreateTopic(_topicId);
        _pubSubFixture.CreateSubscription(_topicId, _subscriptionId);
    }

    [Fact]
    public async Task ShouldReturnSameContractPublisherAndConsumer()
    {
        // Arrange
        var publisher = await _pubSubFixture.CreatePublisherAsync(_topicId);
        var consumerContract = IdentityIntelligenceContracts.Receive.Consumer.IdentityIntelligenceMessage.ReadAsString();
        var providerContract = IdentityIntelligenceContracts.Post.Publisher.IdentityIntelligenceMessage;

        // Act
        await publisher.PublishAsync(providerContract.ReadAsString());
        var receivedMessages =
            await _pubSubFixture.PullMessagesAsync<object>(_subscriptionId, true);

        // Assert
        receivedMessages.Should().NotBeNull();
        consumerContract.Should().BeEquivalentTo(receivedMessages.ReadAsString());
    }
}
