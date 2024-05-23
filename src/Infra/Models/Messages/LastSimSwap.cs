using System.Text.Json.Serialization;

namespace Infra.Models.Messages;

public class LastSimSwap
{
    [JsonPropertyName("lastSimSwapDate")]
    public DateTime? LastSimSwapDate { get; set; }

    [JsonPropertyName("swappedInPeriod")]
    public bool? SwappedInPeriod { get; set; }

    [JsonPropertyName("swappedPeriod")]
    public string? SwappedPeriod { get; set; }
}
