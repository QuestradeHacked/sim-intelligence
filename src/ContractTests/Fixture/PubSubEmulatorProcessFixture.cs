using System.Text.Json;
using ContractTests.Config;
using Google.Cloud.PubSub.V1;
using Grpc.Core;

namespace ContractTests.Fixture;

public class PubSubEmulatorProcessFixture
{
    private readonly AppSettings _appSettings = new();

    private readonly PublisherServiceApiClient _publisherServiceApiClient;

    private readonly SubscriberServiceApiClient _subscriberServiceApiClient;

    public string Endpoint { get; private set; }

    public string ProjectId { get; private set; }

    public int SubscriberTimeout { get; private set; }

    public PubSubEmulatorProcessFixture()
    {
        Endpoint = $"{_appSettings.GetProcessPubSubHost()}:{_appSettings.GetProcessPubSubPort()}";
        Environment.SetEnvironmentVariable("PUBSUB_EMULATOR_HOST", Endpoint);

        ProjectId = _appSettings.GetPubSubProjectId();
        SubscriberTimeout = _appSettings.GetPubSubSubscriberTimeout();

        _publisherServiceApiClient = CreatePublisherServiceApiClient();
        _subscriberServiceApiClient = CreateSubscriberServiceApiClient();
    }

    public Topic CreateTopic(string topicId)
    {
        var topicName = TopicName.FromProjectTopic(ProjectId, topicId);
        var topic = _publisherServiceApiClient.CreateTopic(topicName);

        Console.WriteLine($"Topic {topic.Name} created.");

        return topic;
    }

    public Subscription CreateSubscription(string topicId, string subscriptionId)
    {
        var topicName = TopicName.FromProjectTopic(ProjectId, topicId);
        var subscriptionName = SubscriptionName.FromProjectSubscription(ProjectId, subscriptionId);

        var subscription = _subscriberServiceApiClient.CreateSubscription(
            new Subscription
            {
                TopicAsTopicName = topicName,
                SubscriptionName = subscriptionName,
                EnableMessageOrdering = true
            }
        );

        Console.WriteLine($"Subscription {subscriptionId} created.");

        return subscription;
    }

    public async Task<PublisherClient> CreatePublisherAsync(string topicId)
    {
        var publisherClientBuilder = new PublisherClientBuilder
        {
            ApiSettings = PublisherServiceApiSettings.GetDefault(),
            ChannelCredentials = ChannelCredentials.Insecure,
            Endpoint = Endpoint,
            Settings = new PublisherClient.Settings
            {
                EnableMessageOrdering = true
            },
            TopicName = TopicName.FromProjectTopic(ProjectId, topicId)
        };

        var publisher = await publisherClientBuilder.BuildAsync();

        return publisher;
    }

    public async Task<TData> PullMessagesAsync<TData>(string subscriptionId, bool acknowledge)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var subscriber = await CreateSubscriberAsync(subscriptionId);
        var receivedMessages = new List<TData>();
        var startTask = subscriber.StartAsync((message, _) =>
        {
            var dataJson = message.Data.ToStringUtf8();
            var data = JsonSerializer.Deserialize<TData>(dataJson, serializerOptions);
            receivedMessages.Add(data!);
            return Task.FromResult(acknowledge ? SubscriberClient.Reply.Ack : SubscriberClient.Reply.Nack);
        });

        await Task.Delay(SubscriberTimeout);
        await subscriber.StopAsync(CancellationToken.None);
        await startTask;

        return receivedMessages.First();
    }

    private async Task<SubscriberClient> CreateSubscriberAsync(string subscriptionId)
    {
        var subscriptionName = SubscriptionName.FromProjectSubscription(ProjectId, subscriptionId);
        var subscriberClientBuilder = new SubscriberClientBuilder
        {
            SubscriptionName = subscriptionName,
            ChannelCredentials = ChannelCredentials.Insecure,
            Endpoint = Endpoint,
            Settings = new SubscriberClient.Settings()
        };

        return await subscriberClientBuilder.BuildAsync();
    }

    private PublisherServiceApiClient CreatePublisherServiceApiClient()
    {
        var publisher = new PublisherServiceApiClientBuilder
        {
            ChannelCredentials = ChannelCredentials.Insecure,
            Endpoint = Endpoint
        }.Build();

        return publisher;
    }

    private SubscriberServiceApiClient CreateSubscriberServiceApiClient()
    {
        var subscriber = new SubscriberServiceApiClientBuilder
        {
            ChannelCredentials = ChannelCredentials.Insecure,
            Endpoint = Endpoint
        }.Build();

        return subscriber;
    }
}

