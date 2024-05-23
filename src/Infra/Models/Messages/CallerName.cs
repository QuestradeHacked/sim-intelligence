using System.Text.Json.Serialization;

namespace Infra.Models.Messages;

public class CallerName
{
    [JsonPropertyName("callerType")]
    public string? CallerType { get; set; }

    [JsonPropertyName("errorCode")]
    public int? ErrorCode { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
