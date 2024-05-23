using System.Text.Json.Serialization;

namespace Infra.Models.Messages;

public class IdentityMatch
{
    [JsonPropertyName("addressCountryMatch")]
    public string? AddressCountryMatch { get; set; }

    [JsonPropertyName("addressLinesMatch")]
    public string? AddressLinesMatch { get; set; }

    [JsonPropertyName("cityMatch")]
    public string? CityMatch { get; set; }

    [JsonPropertyName("dateOfBirthMatch")]
    public string? DateOfBirthMatch { get; set; }

    [JsonPropertyName("errorCode")]
    public int? ErrorCode { get; set; }

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("firstNameMatch")]
    public string? FirstNameMatch { get; set; }

    [JsonPropertyName("lastNameMatch")]
    public string? LastNameMatch { get; set; }

    [JsonPropertyName("nationalIdMatch")]
    public string? NationalIdMatch { get; set; }

    [JsonPropertyName("postalCodeMatch")]
    public string? PostalCodeMatch { get; set; }

    [JsonPropertyName("stateMatch")]
    public string? StateMatch { get; set; }

    [JsonPropertyName("summaryScore")]
    public int? SummaryScore { get; set; }

}
