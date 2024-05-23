namespace Infra.Config.Twilio;

public class TwilioConfiguration
{
    public string AccountSid { get; set; } = default!;

    public string ApiKey { get; set; } = default!;

    public string ApiKeySecret { get; set; } = default!;

    public bool Enable { get; set; }

    public string Fields { get; set; } = default!;

    public int Retry { get; set; } = 3;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(AccountSid) || string.IsNullOrWhiteSpace(ApiKey) ||
            string.IsNullOrWhiteSpace(ApiKeySecret) || string.IsNullOrWhiteSpace(Fields))
        {
            throw new InvalidOperationException(
                "The configuration options for the Identity Intelligence TwilioConfiguration is not valid. Please check vault secrets");
        }
    }
}
