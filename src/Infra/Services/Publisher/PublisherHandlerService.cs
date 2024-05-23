using Domain.Constants;
using Domain.Utils;
using Infra.Models.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Questrade.Library.PubSubClientHelper.Primitives;
using Questrade.Library.PubSubClientHelper.Publisher;

namespace Infra.Services.Publisher;

public class PublisherHandlerService : IPublisherHandlerService
{
    private readonly ILogger<PublisherHandlerService> _logger;

    private readonly IPublisherService<PubSubMessage<IdentityIntelligenceResultMessage>> _publisherService;

    private readonly IServiceProvider _serviceProvider;

    public PublisherHandlerService(
        ILogger<PublisherHandlerService> logger,
        IPublisherService<PubSubMessage<IdentityIntelligenceResultMessage>> publisherService,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _publisherService = publisherService;
        _serviceProvider = serviceProvider;
    }

    public async Task Publish(IdentityIntelligenceResultMessage message, CancellationToken cancellationToken)
    {
        var messageToPublish = new PubSubMessage<IdentityIntelligenceResultMessage>
        {
            Data = message,
            Source = PubSubMetadata.Source,
            SpecVersion = IdentityIntelligenceResultPublisherService.SpecVersion,
            Type = IdentityIntelligenceResultPublisherService.Type
        };

        using var scope = _serviceProvider.CreateScope();
        var correlationContext = scope.ServiceProvider.GetRequiredService<CorrelationContext>();

        var publishOptions = new PublishingOptions
        {
            TrackingId = correlationContext?.CorrelationId
        };

        await _publisherService.PublishMessageAsync(messageToPublish, publishOptions);

        _logPublishedMessageInformation(_logger, message.ProfileId, message.CrmUserId, messageToPublish.Id, null);
    }

    private readonly Action<ILogger, string?, string?, string?, Exception?> _logPublishedMessageInformation =
        LoggerMessage.Define<string?, string?, string?>(
            eventId: new EventId(1, nameof(PublisherHandlerService)),
            formatString: "Published message: profile Id {ProfileId}, crm user Id {CrmUserId}, message Id {MessageId}",
            logLevel: LogLevel.Information
        );
}
