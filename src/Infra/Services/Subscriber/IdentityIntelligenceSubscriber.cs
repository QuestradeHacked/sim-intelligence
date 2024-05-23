using Domain.Constants;
using Domain.Services;
using Domain.Utils;
using FluentValidation;
using Infra.Config.PubSub;
using Infra.Models.Messages;
using Infra.Services.Publisher;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Questrade.Library.PubSubClientHelper.Subscriber;

namespace Infra.Services.Subscriber;

public class IdentityIntelligenceSubscriber : PubsubSubscriberBackgroundService<
    IdentityIntelligenceSubscriberConfiguration, IdentityIntelligenceMessage>
{
    private readonly IMediator _mediator;

    private readonly IMetricService _metricService;

    private readonly IPublisherHandlerService _publisherService;

    private readonly IServiceProvider _serviceProvider;

    private readonly IValidator<IdentityIntelligenceMessage> _validator;

    public IdentityIntelligenceSubscriber(
        ILoggerFactory loggerFactory,
        IMediator mediator,
        IMetricService metricService,
        IPublisherHandlerService publisherService,
        IServiceProvider serviceProvider,
        IdentityIntelligenceSubscriberConfiguration subscriberConfiguration,
        IValidator<IdentityIntelligenceMessage> validator)
        : base(loggerFactory, subscriberConfiguration, serviceProvider)
    {
        _mediator = mediator;
        _metricService = metricService;
        _publisherService = publisherService;
        _serviceProvider = serviceProvider;
        _validator = validator;

        ReportMessageReceived += OnReportMessageReceived;
    }

    protected override async Task<bool> HandleReceivedMessageAsync(IdentityIntelligenceMessage message,
        CancellationToken cancellationToken)
    {
        _logDefineScope(Logger, nameof(IdentityIntelligenceSubscriber), nameof(HandleReceivedMessageAsync));
        _logMessageReceivedInformation(Logger, message.Id, null);

        _metricService.Increment(MetricNames.IdentityIntelligenceReceivedMessageCount,
            new List<string> { MetricTags.StatusSuccess });

        var validatorResult = await _validator.ValidateAsync(message, cancellationToken);

        if (!validatorResult.IsValid)
        {
            _logMissingRequiredInformationWarning(Logger,
                string.Join(';', validatorResult.Errors.Select(e => e.ErrorMessage)), null);

            return true;
        }

        try
        {
            var identityIntelligenceScanResult = await _mediator.Send(message, cancellationToken);

            if (identityIntelligenceScanResult is null)
            {
                return false;
            }

            await _publisherService.Publish(identityIntelligenceScanResult, cancellationToken);

            _metricService.Increment(MetricNames.IdentityIntelligenceHandleMessageCount,
                new List<string> { MetricTags.StatusSuccess });

            return true;
        }
        catch (Exception error)
        {
            _logHandlingMessageError(Logger, nameof(IdentityIntelligenceSubscriber), message.Id, error);

            _metricService.Increment(MetricNames.IdentityIntelligenceHandleMessageCount,
                new List<string> { MetricTags.StatusPermanentError });

            return false;
        }
    }

    private void OnReportMessageReceived(object? sender,
        Questrade.Library.PubSubClientHelper.Events.MessageReceivedEventArgs e)
    {
        if (e.Message?.Attributes.ContainsKey("tr-id") == false || e.Message?.Attributes["tr-id"] is null)
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var correlationContext = scope.ServiceProvider.GetRequiredService<CorrelationContext>();

        correlationContext.AddLogScopeForSubscriber(Logger, e.Message.Attributes["tr-id"], e.Message.MessageId);
    }

    private readonly Func<ILogger, string?, string?, IDisposable> _logDefineScope =
        LoggerMessage.DefineScope<string?, string?>(
            formatString: "{IdentityIntelligenceSubscriberName}:{HandleReceivedMessageAsyncName}"
        );

    private readonly Action<ILogger, string?, Exception?> _logMessageReceivedInformation =
        LoggerMessage.Define<string?>(
            eventId: new EventId(1, nameof(IdentityIntelligenceSubscriber)),
            formatString: "Message received: {MessageId}",
            logLevel: LogLevel.Information
        );

    private readonly Action<ILogger, string?, Exception?> _logMissingRequiredInformationWarning =
        LoggerMessage.Define<string?>(
            eventId: new EventId(2, nameof(IdentityIntelligenceSubscriber)),
            formatString: "{ValidationMessage}",
            logLevel: LogLevel.Warning
        );

    private readonly Action<ILogger, string?, string?, Exception?> _logHandlingMessageError =
        LoggerMessage.Define<string?, string?>(
            eventId: new EventId(3, nameof(IdentityIntelligenceSubscriber)),
            formatString:
            "Failed on handling the received message from {IdentityIntelligenceSubscriberName}: {MessageId}",
            logLevel: LogLevel.Error
        );
}
