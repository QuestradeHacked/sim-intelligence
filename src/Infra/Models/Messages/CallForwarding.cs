using System.Text.Json.Serialization;

namespace Infra.Models.Messages;

public class CallForwarding
{
    [JsonPropertyName("callForwardingStatus")]
    public bool? CallForwardingStatus { get; set; }

    [JsonPropertyName("carrierName")]
    public string? CarrierName { get; set; }

    [JsonPropertyName("errorCode")]
    public int? ErrorCode { get; set; }

    [JsonPropertyName("mobileCountryCode")]
    public string? MobileCountryCode { get; set; }

    [JsonPropertyName("mobileNetworkCode")]
    public string? MobileNetworkCode { get; set; }
}
