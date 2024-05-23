using Infra.Config.PubSub;
using Infra.Config.Twilio;

namespace WebApi.Config;

public class IdentityIntelligenceConfiguration
{
    public IdentityIntelligenceResultPublisherConfiguration IdentityIntelligenceResultPublisherConfiguration { get; set; } = new();

    public IdentityIntelligenceSubscriberConfiguration IdentityIntelligenceSubscriberConfiguration { get; set; } = new();

    public TwilioConfiguration TwilioConfiguration { get; set; } = new();

    internal void Validate()
    {
        if (IdentityIntelligenceResultPublisherConfiguration == null)
        {
            throw new InvalidOperationException("Identity intelligence result publisher configuration is not valid.");
        }

        IdentityIntelligenceResultPublisherConfiguration.Validate();

        if (IdentityIntelligenceSubscriberConfiguration == null)
        {
            throw new InvalidOperationException("Identity intelligence subscriber configuration is not valid.");
        }

        IdentityIntelligenceSubscriberConfiguration.Validate();

        if (TwilioConfiguration == null)
        {
            throw new InvalidOperationException("Twilio configuration is not valid.");
        }

        TwilioConfiguration.Validate();
    }
}
