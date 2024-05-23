using System.Text.Json.Serialization;

namespace Infra.Models.Messages;

public class RoamingCarrier : Carrier
{
    [JsonPropertyName("country")]
    public string? Country { get; set; }
}
