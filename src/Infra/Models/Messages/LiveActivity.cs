using System.Text.Json.Serialization;

namespace Infra.Models.Messages;

public class LiveActivity
{
    [JsonPropertyName("connectivity")]
    public string? Connectivity { get; set; }

    [JsonPropertyName("originalCarrier")]
    public Carrier? OriginalCarrier { get; set; }

    [JsonPropertyName("ported")]
    public bool? Ported { get; set; }

    [JsonPropertyName("portedCarrier")]
    public Carrier? PortedCarrier { get; set; }

    [JsonPropertyName("roamingCarrier")]
    public RoamingCarrier? RoamingCarrier { get; set; }
}
