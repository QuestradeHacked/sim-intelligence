using Infra.Models.Messages;
using Microsoft.Extensions.Logging;
using Questrade.Library.PubSubClientHelper.Primitives;

namespace Infra.Config.PubSub;

public class IdentityIntelligenceSubscriberConfiguration : BaseSubscriberConfiguration<IdentityIntelligenceMessage>
{
    public override Task HandleMessageLogAsync(ILogger logger, LogLevel logLevel, IdentityIntelligenceMessage message, string logMessage, Exception? error = null, CancellationToken cancellationToken = default)
    {
        logger.Log(logLevel, error, "{LogMessage} - Message with: {MessageId}", logMessage, message.Id);

        return Task.CompletedTask;
    }

    public void Validate()
    {
        if (!Enable) return;

        if (string.IsNullOrWhiteSpace(ProjectId) || string.IsNullOrWhiteSpace(SubscriptionId))
        {
            throw new InvalidOperationException("The configuration options for the IdentityIntelligenceSubscriber is not valid");
        }

        if (UseEmulator && string.IsNullOrWhiteSpace(Endpoint))
        {
            throw new InvalidOperationException("The emulator configuration options for IdentityIntelligenceSubscriber is not valid");
        }
    }
}
