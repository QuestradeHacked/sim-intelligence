using System.Text.Json.Serialization;

namespace Infra.Models.Messages;

public class LineTypeIntelligence
{
    [JsonPropertyName("carrierName")]
    public string? CarrierName { get; set; }

    public int? ErrorCode { get; set; }

    [JsonPropertyName("mobileCountryCode")]
    public string? MobileCountryCode { get; set; }

    [JsonPropertyName("mobileNetworkCode")]
    public string? MobileNetworkCode { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
