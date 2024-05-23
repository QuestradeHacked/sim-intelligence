namespace Domain.Constants;

public static class MetricNames
{
    public const string IdentityIntelligenceHandleMessageCount = "identity.intelligence.subscriber_handle_message_count";

    public const string IdentityIntelligenceReceivedMessageCount = "identity.intelligence.subscriber_received_message_count";

    public const string TwilioApiCallErrorCount = "twilio.request.handler_handle_message_twilio_error_count";

    public const string TwilioApiCallTimer = "twilio.request.handler_handle_message_twilio_latency";
}
