using Domain.Constants;
using Domain.Services;
using Infra.Config.Twilio;
using Infra.Extensions;
using Infra.Models.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using SerilogTimings;
using Twilio;
using Twilio.Rest.Lookups.V2;

namespace Application.Handlers;

public class TwilioRequestHandler : IRequestHandler<IdentityIntelligenceMessage, IdentityIntelligenceResultMessage>
{
    private readonly ILogger<TwilioRequestHandler> _logger;

    private readonly IMetricService _metricService;

    private readonly TwilioConfiguration _twilioConfiguration;

    public TwilioRequestHandler(
        ILogger<TwilioRequestHandler> logger,
        IMetricService metricService,
        TwilioConfiguration twilioConfiguration
    )
    {
        _logger = logger;
        _metricService = metricService;
        _twilioConfiguration = twilioConfiguration;
    }

    public async Task<IdentityIntelligenceResultMessage> Handle(IdentityIntelligenceMessage identityIntelligenceMessage,
        CancellationToken cancellationToken)
    {
        if (!_twilioConfiguration.Enable)
        {
            _logTwilioDisabledWarning(_logger, nameof(TwilioRequestHandler), identityIntelligenceMessage.Id, null);

            return null!;
        }

        try
        {
            var retryPolicy = Policy
                .Handle<IOException>()
                .WaitAndRetryAsync(_twilioConfiguration.Retry,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            using var operation = Operation.Begin("Twilio fetching for {MessageId}", identityIntelligenceMessage.Id);

            TwilioClient.Init(_twilioConfiguration.ApiKey, _twilioConfiguration.ApiKeySecret,
                _twilioConfiguration.AccountSid);

            PhoneNumberResource? phoneNumberDetails = null;

            await retryPolicy.ExecuteAsync(async () =>
            {
                phoneNumberDetails = await PhoneNumberResource.FetchAsync(
                    fields: _twilioConfiguration.Fields,
                    pathPhoneNumber: identityIntelligenceMessage.PhoneNumber,
                    firstName: identityIntelligenceMessage.FirstName,
                    lastName: identityIntelligenceMessage.LastName,
                    addressLine1: identityIntelligenceMessage.AddressLine1,
                    addressLine2: identityIntelligenceMessage.AddressLine2,
                    city: identityIntelligenceMessage.City,
                    state: identityIntelligenceMessage.State,
                    postalCode: identityIntelligenceMessage.PostalCode,
                    addressCountryCode: identityIntelligenceMessage.AddressCountryCode,
                    nationalId: identityIntelligenceMessage.NationalId,
                    dateOfBirth: identityIntelligenceMessage.DateOfBirth
                );
            });

            operation.Complete();

            _metricService.Distribution(MetricNames.TwilioApiCallTimer, operation.Elapsed.TotalMilliseconds);

            return new IdentityIntelligenceResultMessage
            {
                AccountNumber = identityIntelligenceMessage.AccountNumber,
                AccountStatusId = identityIntelligenceMessage.AccountStatusId,
                CrmUserId = identityIntelligenceMessage.CrmUserId,
                EffectiveDate = identityIntelligenceMessage.EffectiveDate,
                EnterpriseProfileId = identityIntelligenceMessage.EnterpriseProfileId,
                ProfileId = identityIntelligenceMessage.ProfileId,
                ScanResult = phoneNumberDetails?.ParseScanResult()
            };
        }
        catch (Exception error)
        {
            _logTwilioError(_logger, identityIntelligenceMessage.Id, error);
            _metricService.Increment(
                statName: MetricNames.TwilioApiCallErrorCount,
                tags: new List<string> { MetricTags.StatusPermanentError }
            );

            return null!;
        }
    }

    private readonly Action<ILogger, string?, string?, Exception?> _logTwilioDisabledWarning =
        LoggerMessage.Define<string?, string?>(
            eventId: new EventId(1, nameof(TwilioRequestHandler)),
            formatString: "Twilio not enabled for {TwilioRequestHandlerName} with {MessageId}",
            logLevel: LogLevel.Warning
        );

    private readonly Action<ILogger, string?, Exception?> _logTwilioError =
        LoggerMessage.Define<string?>(
            eventId: new EventId(2, nameof(TwilioRequestHandler)),
            formatString: "Failed on fetching phone number details from Twilio with {MessageId}",
            logLevel: LogLevel.Error
        );
}
