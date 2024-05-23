using Microsoft.Extensions.Logging;

namespace Domain.Utils;

public class CorrelationContext
{
    public string CorrelationId { get; private set; }

    public CorrelationContext()
    {
        CorrelationId = Guid.NewGuid().ToString();
    }

    public void AddLogScopeForSubscriber(
        ILogger logger,
        string? correlationId,
        string? messageId)
    {
        if (correlationId is not null)
        {
            CorrelationId = correlationId;
        }

        LogSubscriberDefineScope(logger, CorrelationId, messageId);
    }

    private static readonly Func<ILogger, string?, string?, IDisposable>
        LogSubscriberDefineScope =
            LoggerMessage.DefineScope<string?, string?>(
                "PubSubSubscriber {CorrelationId} {MessageId}"
            );
}
