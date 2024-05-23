using Infra.Models.Messages;
using Microsoft.Extensions.Logging;
using Questrade.Library.PubSubClientHelper.Primitives;

namespace Infra.Config.PubSub;

public class
    IdentityIntelligenceResultPublisherConfiguration : BasePublisherConfiguration<
        PubSubMessage<IdentityIntelligenceResultMessage>>
{
    public override Task HandleMessageLogAsync(ILogger logger, LogLevel logLevel,
        PubSubMessage<IdentityIntelligenceResultMessage> message, string logMessage,
        CancellationToken cancellationToken = default)
    {
        logger.Log(logLevel, "{LogMessage} - Message with: {MessageId}", logMessage, message.Id);

        return Task.CompletedTask;
    }

    public virtual void Validate()
    {
        if (!Enable) return;

        if (string.IsNullOrWhiteSpace(ProjectId))
        {
            throw new InvalidOperationException(
                $"The ProjectId : {ProjectId} configuration options for the IdentityIntelligenceResultPublisher is not valid");
        }

        if (string.IsNullOrWhiteSpace(TopicId))
        {
            throw new InvalidOperationException(
                $"The TopicId : {TopicId} configuration option for the IdentityIntelligenceResultPublisher is not valid");
        }

        if (UseEmulator && string.IsNullOrWhiteSpace(Endpoint))
        {
            throw new InvalidOperationException(
                $"The Endpoint: {Endpoint} emulator configuration options for IdentityIntelligenceResultPublisher is not valid");
        }
    }
}
