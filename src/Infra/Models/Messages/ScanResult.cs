using System.Text.Json.Serialization;

namespace Infra.Models.Messages;

public class ScanResult
{
    [JsonPropertyName("callerName")]
    public CallerName? CallerName { get; set; }

    [JsonPropertyName("callForwarding")]
    public CallForwarding? CallForwarding { get; set; }

    [JsonPropertyName("callingCountryCode")]
    public string? CallingCountryCode { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("identityMatch")]
    public IdentityMatch? IdentityMatch { get; set; }

    [JsonPropertyName("lineTypeIntelligence")]
    public LineTypeIntelligence? LineTypeIntelligence { get; set; }

    [JsonPropertyName("liveActivity")]
    public LiveActivity? LiveActivity { get; set; }

    [JsonPropertyName("nationalFormat")]
    public string? NationalFormat { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("simSwap")]
    public SimSwap? SimSwap { get; set; }

    [JsonPropertyName("url")]
    public Uri? Url { get; set; }

    [JsonPropertyName("valid")]
    public bool? Valid { get; set; }

    [JsonPropertyName("validationErrors")]
    public IEnumerable<string>? ValidationErrors { get; set; }

}
