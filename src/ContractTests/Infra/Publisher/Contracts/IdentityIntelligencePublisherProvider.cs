using Infra.Models.Messages;

namespace ContractTests.Infra.Publisher.Contracts;

public class IdentityIntelligencePublisherProvider
{
    public PubSubMessage<IdentityIntelligenceResultMessage> IdentityIntelligenceMessage =>
        new()
        {
            SpecVersion = "1.0",
            Type = "system.financial-crime.internal-scan-result-status.updated-1.0",
            Source = "fc-identity-intelligence",
            Id = "f4dd28cf-b21d-47db-87e3-f80363ed425f",
            Time = DateTime.Parse("2023-01-05T04:30:00.005"),
            DataContentType = "application/json",
            Data = new IdentityIntelligenceResultMessage
            {
                AccountNumber = "123",
                AccountStatusId = 23,
                EffectiveDate = new DateTime(2023, 6, 13),
                EnterpriseProfileId = "54f9365d-9ed0-4eca-9d3f-f0837c34a309",
                CrmUserId = "3000050",
                ProfileId = "54f9365d-9ed0-4eca-9d3f-f0837c34a309",
                ScanResult = new ScanResult
                {
                    PhoneNumber = "+12345678901",
                    ValidationErrors = new[] { "TOO_SHORT" },
                    CallForwarding = new CallForwarding
                    {
                        CarrierName = "Mobile Company S.A",
                        MobileCountryCode = "123",
                        MobileNetworkCode = "12",
                        CallForwardingStatus = true,
                        ErrorCode = 6060
                    },
                    CallingCountryCode = "1",
                    LiveActivity = new LiveActivity
                    {
                        Connectivity = "connected",
                        Ported = true,
                        PortedCarrier = new Carrier
                        {
                            Name = "Mobile Company S.A",
                            MobileCountryCode = "123",
                            MobileNetworkCode = "2"
                        },
                        RoamingCarrier = new RoamingCarrier
                        {
                            Name = "Mobile Company S.A",
                            MobileCountryCode = "123",
                            MobileNetworkCode = "12",
                            Country = "CA"
                        },
                        OriginalCarrier = new Carrier
                        {
                            Name = "Mobile Company S.A",
                            MobileCountryCode = "123",
                            MobileNetworkCode = "12"
                        }
                    },
                    SimSwap = new SimSwap
                    {
                        CarrierName = "Mobile Company S.A",
                        ErrorCode = 6060,
                        MobileCountryCode = "123",
                        MobileNetworkCode = "12",
                        LastSimSwap = new LastSimSwap
                        {
                            LastSimSwapDate = DateTime.Parse("2020-04-27T06:18:50.048"),
                            SwappedPeriod = "PT15282H33M44S",
                            SwappedInPeriod = true
                        }
                    },
                    CallerName = new CallerName
                    {
                        Name = "Sergio Suarez",
                        CallerType = "CONSUMER",
                        ErrorCode = 6060
                    },
                    NationalFormat = "(123) 456-789 ",
                    CountryCode = "CA",
                    IdentityMatch = new IdentityMatch
                    {
                        AddressLinesMatch = "exact_match",
                        AddressCountryMatch = "exact_match",
                        CityMatch = "exact_match",
                        DateOfBirthMatch = "exact_match",
                        ErrorCode = 0,
                        ErrorMessage = "no error",
                        FirstNameMatch = "exact_match",
                        LastNameMatch = "exact_match",
                        NationalIdMatch = "exact_match",
                        PostalCodeMatch = "exact_match",
                        StateMatch = "exact_match",
                        SummaryScore = 0
                    },
                    LineTypeIntelligence = new LineTypeIntelligence
                    {
                        MobileNetworkCode = "12",
                        CarrierName = "Mobile Company S.A",
                        ErrorCode = 6060,
                        MobileCountryCode = "123",
                        Type = "mobile"
                    },
                    Valid = true,
                    Url = new("http://localhost/")
                }
            }
        };
}
