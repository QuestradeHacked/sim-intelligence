using System.Text.Json.Serialization;

namespace Infra.Models.Messages;

public class IdentityIntelligenceResultMessage
{
    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; set; }

    [JsonPropertyName("accountStatusId")]
    public int? AccountStatusId { get; set; }

    [JsonPropertyName("crmUserId")]
    public string? CrmUserId { get; set; }

    [JsonPropertyName("effectiveDate")]
    public DateTime? EffectiveDate { get; set; }

    [JsonPropertyName("enterpriseProfileId")]
    public string? EnterpriseProfileId { get; set; }

    [JsonPropertyName("profileId")]
    public string? ProfileId { get; set; }

    [JsonPropertyName("scanResult")]
    public ScanResult? ScanResult { get; set; }

}
