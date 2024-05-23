using System.Text.Json.Serialization;

namespace Infra.Models.Messages;

public class Carrier
{
    [JsonPropertyName("mobileCountryCode")]
    public string? MobileCountryCode { get; set; }

    [JsonPropertyName("mobileNetworkCode")]
    public string? MobileNetworkCode { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
