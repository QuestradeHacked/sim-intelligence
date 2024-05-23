using Infra.Config.PubSub;
using Infra.Models.Messages;
using Microsoft.Extensions.Logging;
using Questrade.Library.PubSubClientHelper.Primitives;
using Questrade.Library.PubSubClientHelper.Publisher.Outbox;

namespace Infra.Services.Publisher;

public class IdentityIntelligenceResultPublisherService : OutboxPubsubPublisherService<IdentityIntelligenceResultPublisherConfiguration, PubSubMessage<IdentityIntelligenceResultMessage>>
{
    public static string? SpecVersion;
    public static string? Type;

    public IdentityIntelligenceResultPublisherService(
        ILoggerFactory logger,
        IdentityIntelligenceResultPublisherConfiguration publisherConfiguration,
        IMessageOutboxStore<PubSubMessage<IdentityIntelligenceResultMessage>> defaultJsonSerializerOptionsProvider
    )
        : base(logger, publisherConfiguration, defaultJsonSerializerOptionsProvider)
    {
        SpecVersion = string.Join("", publisherConfiguration.TopicId?.Split("-").LastOrDefault()?.Take(3) ?? Array.Empty<char>());
        Type = publisherConfiguration.TopicId;
    }
}
