namespace ContractTests.Infra.Publisher.Contracts;

public class IdentityIntelligencePublisherConsumer
{
    public object IdentityIntelligenceMessage =>
        new
        {
            data = new
            {
                accountNumber = "123",
                accountStatusId = 23,
                crmUserId = "3000050",
                effectiveDate = new DateTime(2023,6,13),
                enterpriseProfileId = "54f9365d-9ed0-4eca-9d3f-f0837c34a309",
                profileId = "54f9365d-9ed0-4eca-9d3f-f0837c34a309",
                scanResult = new
                {
                    callerName = new
                    {
                        callerType = "CONSUMER",
                        errorCode = 6060,
                        name = "Sergio Suarez"
                    },
                    callForwarding = new
                    {
                        callForwardingStatus = true,
                        carrierName = "Mobile Company S.A",
                        errorCode = 6060,
                        mobileCountryCode = "123",
                        mobileNetworkCode = "12"
                    },
                    callingCountryCode = "1",
                    countryCode = "CA",
                    identityMatch = new
                    {
                        addressCountryMatch = "exact_match",
                        addressLinesMatch = "exact_match",
                        cityMatch = "exact_match",
                        dateOfBirthMatch = "exact_match",
                        errorCode = 0,
                        errorMessage = "no error",
                        firstNameMatch = "exact_match",
                        lastNameMatch = "exact_match",
                        nationalIdMatch = "exact_match",
                        postalCodeMatch = "exact_match",
                        stateMatch = "exact_match",
                        summaryScore = 0
                    },
                    lineTypeIntelligence = new
                    {
                        carrierName = "Mobile Company S.A",
                        errorCode = 6060,
                        mobileCountryCode = "123",
                        mobileNetworkCode = "12",
                        type = "mobile"
                    },
                    liveActivity = new
                    {
                        connectivity = "connected",
                        originalCarrier = new
                        {
                            mobileCountryCode = "123",
                            mobileNetworkCode = "12",
                            name = "Mobile Company S.A"
                        },
                        ported = true,
                        portedCarrier = new
                        {
                            mobileCountryCode = "123",
                            mobileNetworkCode = "2",
                            name = "Mobile Company S.A"
                        },
                        roamingCarrier = new
                        {
                            country = "CA",
                            mobileCountryCode = "123",
                            mobileNetworkCode = "12",
                            name = "Mobile Company S.A"
                        }
                    },
                    nationalFormat = "(123) 456-789 ",
                    phoneNumber = "\u002B12345678901",
                    simSwap = new
                    {
                        carrierName = "Mobile Company S.A",
                        errorCode = 6060,
                        lastSimSwap = new
                        {
                            lastSimSwapDate = "2020-04-27T06:18:50.048",
                            swappedInPeriod = true,
                            swappedPeriod = "PT15282H33M44S"
                        },
                        mobileCountryCode = "123",
                        mobileNetworkCode = "12"
                    },
                    url = "http://localhost/",
                    valid = true,
                    validationErrors = new[] { "TOO_SHORT" }
                }
            },
            dataContentType = "application/json",
            id = "f4dd28cf-b21d-47db-87e3-f80363ed425f",
            source = "fc-identity-intelligence",
            specVersion = "1.0",
            time = "2023-01-05T04:30:00.005",
            type = "system.financial-crime.internal-scan-result-status.updated-1.0"
        };
}
