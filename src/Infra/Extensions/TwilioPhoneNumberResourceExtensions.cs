using Domain.Constants;
using Infra.Models.Messages;
using Newtonsoft.Json.Linq;
using Twilio.Rest.Lookups.V2;

namespace Infra.Extensions;

public static class TwilioPhoneNumberResourceExtensions
{
    public static ScanResult ParseScanResult(this PhoneNumberResource twilioResource)
    {
        return new ScanResult
        {
            CallingCountryCode = twilioResource.CallingCountryCode,
            CountryCode = twilioResource.CountryCode,
            CallerName = twilioResource.ParseCallerName(),
            CallForwarding = twilioResource.ParseCallForwarding(),
            LineTypeIntelligence = twilioResource.ParseLineTypeIntelligence(),
            LiveActivity = twilioResource.ParseLiveActivity(),
            NationalFormat = twilioResource.NationalFormat,
            PhoneNumber = twilioResource.PhoneNumber.ToString(),
            SimSwap = twilioResource.ParseSimSwap(),
            IdentityMatch = twilioResource.ParseIdentityMatch(),
            Url = twilioResource.Url,
            Valid = twilioResource.Valid,
            ValidationErrors = twilioResource.ValidationErrors.Select(x => x.ToString()).AsEnumerable()
        };
    }

    private static CallerName? ParseCallerName(this PhoneNumberResource twilioPhoneNumberResource)
    {
        var twilioCallerNameSource = (JObject?)twilioPhoneNumberResource.CallerName;

        if (twilioCallerNameSource is not { HasValues: true })
        {
            return null;
        }

        return new CallerName
        {
            CallerType = (string?)twilioCallerNameSource.Property(TwilioPropertyNames.CallerType),
            ErrorCode = (int?)twilioCallerNameSource.Property(TwilioPropertyNames.ErrorCode),
            Name = (string?)twilioCallerNameSource.Property(TwilioPropertyNames.CallerName)
        };
    }

    private static CallForwarding? ParseCallForwarding(this PhoneNumberResource twilioPhoneNumberResource)
    {
        var twilioCallForwardingSource = (JObject?)twilioPhoneNumberResource.CallForwarding;

        if (twilioCallForwardingSource is not { HasValues: true })
        {
            return null;
        }

        return new CallForwarding
        {
            CallForwardingStatus = (bool?)twilioCallForwardingSource.Property(TwilioPropertyNames.CallForwardingStatus),
            CarrierName = (string?)twilioCallForwardingSource.Property(TwilioPropertyNames.CarrierName),
            ErrorCode = (int?)twilioCallForwardingSource.Property(TwilioPropertyNames.ErrorCode),
            MobileCountryCode = (string?)twilioCallForwardingSource.Property(TwilioPropertyNames.MobileCountryCode),
            MobileNetworkCode = (string?)twilioCallForwardingSource.Property(TwilioPropertyNames.MobileNetworkCode)
        };
    }

    private static LineTypeIntelligence? ParseLineTypeIntelligence(this PhoneNumberResource twilioPhoneNumberResource)
    {
        var twilioLineTypeIntelligenceSource = (JObject?)twilioPhoneNumberResource.LineTypeIntelligence;

        if (twilioLineTypeIntelligenceSource is not { HasValues: true })
        {
            return null;
        }

        return new LineTypeIntelligence
        {
            CarrierName = (string?)twilioLineTypeIntelligenceSource.Property(TwilioPropertyNames.CarrierName),
            ErrorCode = (int?)twilioLineTypeIntelligenceSource.Property(TwilioPropertyNames.ErrorCode),
            MobileCountryCode =
                (string?)twilioLineTypeIntelligenceSource.Property(TwilioPropertyNames.MobileCountryCode),
            MobileNetworkCode =
                (string?)twilioLineTypeIntelligenceSource.Property(TwilioPropertyNames.MobileNetworkCode),
            Type = (string?)twilioLineTypeIntelligenceSource.Property(TwilioPropertyNames.Type)
        };
    }

    private static LiveActivity? ParseLiveActivity(this PhoneNumberResource twilioPhoneNumberResource)
    {
        var twilioLiveActivitySource = (JObject?)twilioPhoneNumberResource.LiveActivity;

        if (twilioLiveActivitySource is not { HasValues: true })
        {
            return null;
        }

        var liveActivity = new LiveActivity
        {
            Connectivity = (string?)twilioLiveActivitySource.Property(TwilioPropertyNames.Connectivity),
            Ported = (bool?)twilioLiveActivitySource.Property(TwilioPropertyNames.Ported)
        };
        var twilioOriginalCarrierSource = twilioLiveActivitySource.Property(TwilioPropertyNames.OriginalCarrier);
        var twilioPortedCarrierSource = twilioLiveActivitySource.Property(TwilioPropertyNames.PortedCarrier);
        var twilioRoamingCarrierSource = twilioLiveActivitySource.Property(TwilioPropertyNames.RoamingCarrier);

        if (twilioOriginalCarrierSource is { Value.HasValues: true })
        {
            liveActivity.OriginalCarrier = new Carrier
            {
                MobileCountryCode =
                    twilioOriginalCarrierSource.Value.Value<string?>(TwilioPropertyNames.MobileCountryCode),
                MobileNetworkCode =
                    twilioOriginalCarrierSource.Value.Value<string?>(TwilioPropertyNames.MobileNetworkCode),
                Name = twilioOriginalCarrierSource.Value.Value<string?>(TwilioPropertyNames.Name)
            };
        }

        if (twilioPortedCarrierSource is { Value.HasValues: true })
        {
            liveActivity.PortedCarrier = new Carrier
            {
                MobileCountryCode =
                    twilioPortedCarrierSource.Value.Value<string?>(TwilioPropertyNames.MobileCountryCode),
                MobileNetworkCode =
                    twilioPortedCarrierSource.Value.Value<string?>(TwilioPropertyNames.MobileNetworkCode),
                Name = twilioPortedCarrierSource.Value.Value<string?>(TwilioPropertyNames.Name)
            };
        }

        if (twilioRoamingCarrierSource is { Value.HasValues: true })
        {
            liveActivity.RoamingCarrier = new RoamingCarrier
            {
                Country = twilioRoamingCarrierSource.Value.Value<string?>(TwilioPropertyNames.Country),
                MobileCountryCode =
                    twilioRoamingCarrierSource.Value.Value<string?>(TwilioPropertyNames.MobileCountryCode),
                MobileNetworkCode =
                    twilioRoamingCarrierSource.Value.Value<string?>(TwilioPropertyNames.MobileNetworkCode),
                Name = twilioRoamingCarrierSource.Value.Value<string?>(TwilioPropertyNames.Name)
            };
        }

        return liveActivity;
    }

    private static SimSwap? ParseSimSwap(this PhoneNumberResource twilioPhoneNumberResource)
    {
        var twilioSimSwapSource = (JObject?)twilioPhoneNumberResource.SimSwap;

        if (twilioSimSwapSource is not { HasValues: true })
        {
            return null;
        }

        var simSwap = new SimSwap
        {
            CarrierName = (string?)twilioSimSwapSource.Property(TwilioPropertyNames.CarrierName),
            ErrorCode = (int?)twilioSimSwapSource.Property(TwilioPropertyNames.ErrorCode),
            MobileCountryCode = (string?)twilioSimSwapSource.Property(TwilioPropertyNames.MobileCountryCode),
            MobileNetworkCode = (string?)twilioSimSwapSource.Property(TwilioPropertyNames.MobileNetworkCode)
        };

        var twilioLastSimSwapSource = twilioSimSwapSource.Property(TwilioPropertyNames.LastSimSwap);

        if (twilioLastSimSwapSource is not { Value.HasValues: true })
        {
            return simSwap;
        }

        var dateString = twilioLastSimSwapSource.Value.Value<string?>(TwilioPropertyNames.LastSimSwapDate);
        simSwap.LastSimSwap = new LastSimSwap
        {
            LastSimSwapDate = Convert.ToDateTime(dateString),
            SwappedInPeriod = twilioLastSimSwapSource.Value.Value<bool?>(TwilioPropertyNames.SwappedInPeriod),
            SwappedPeriod = twilioLastSimSwapSource.Value.Value<string?>(TwilioPropertyNames.SwappedPeriod)
        };

        return simSwap;
    }

    private static IdentityMatch? ParseIdentityMatch(this PhoneNumberResource twilioPhoneNumberResource)
    {
        var twilioIdentityMatchSource = (JObject?)twilioPhoneNumberResource.IdentityMatch;

        if (twilioIdentityMatchSource is not { HasValues: true })
        {
            return null;
        }

        var identityMatch = new IdentityMatch
        {
            FirstNameMatch = (string?)twilioIdentityMatchSource.Property(TwilioPropertyNames.FirstNameMatch),
            LastNameMatch = (string?)twilioIdentityMatchSource.Property(TwilioPropertyNames.LastNameMatch),
            AddressLinesMatch = (string?)twilioIdentityMatchSource.Property(TwilioPropertyNames.AddressLinesMatch),
            CityMatch = (string?)twilioIdentityMatchSource.Property(TwilioPropertyNames.CityMatch),
            StateMatch = (string?)twilioIdentityMatchSource.Property(TwilioPropertyNames.StateMatch),
            AddressCountryMatch = (string?)twilioIdentityMatchSource.Property(TwilioPropertyNames.AddressCountryMatch),
            PostalCodeMatch = (string?)twilioIdentityMatchSource.Property(TwilioPropertyNames.PostaCodeMatch),
            NationalIdMatch = (string?)twilioIdentityMatchSource.Property(TwilioPropertyNames.NationalIdMatch),
            DateOfBirthMatch = (string?)twilioIdentityMatchSource.Property(TwilioPropertyNames.DateOfBirthMatch),
            ErrorCode = (int?)twilioIdentityMatchSource.Property(TwilioPropertyNames.ErrorCode),
            ErrorMessage = (string?)twilioIdentityMatchSource.Property(TwilioPropertyNames.ErrorMessage),
            SummaryScore = (int?)twilioIdentityMatchSource.Property(TwilioPropertyNames.SummaryScore)
        };
        return identityMatch;
    }
}
