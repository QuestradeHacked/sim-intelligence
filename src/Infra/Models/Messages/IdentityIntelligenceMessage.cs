using System.Text.RegularExpressions;
using MediatR;

namespace Infra.Models.Messages;

public class IdentityIntelligenceMessage : IRequest<IdentityIntelligenceResultMessage>
{
    public string? AddressCountryCode { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AccountNumber { get; set; }
    public int? AccountStatusId { get; set; }
    public string? City { get; set; }
    public string? CrmUserId { get; set; }
    public string? DateOfBirth { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public string? EnterpriseProfileId { get; set; }
    public string? FirstName { get; set; }
    public string Id { get; set; } = default!;
    public string? LastName { get; set; }
    public string? NationalId { get; set; }
    public string? Phone { get; set; }
    public string PhoneNumber => Regex.Replace(Phone ?? string.Empty, @"[^0-9+]", "");
    public string? PostalCode { get; set; }
    public string? ProfileId { get; set; }
    public string? State { get; set; }
}
