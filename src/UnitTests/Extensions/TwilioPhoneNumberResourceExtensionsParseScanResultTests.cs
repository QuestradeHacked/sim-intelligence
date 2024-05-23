using FluentAssertions;
using Infra.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Twilio.Rest.Lookups.V2;
using UnitTests.Faker;
using Xunit;

namespace UnitTests.Extensions;

public class TwilioPhoneNumberResourceExtensionsParseScanResultTests
{
    [Fact]
    public void ParseScanResult_ShouldParseResultSuccessfully_ForTwilioObjectPrimitiveTypes()
    {
        // Arrange
        var phoneNumberResourceFaker = new JObjectPhoneNumberResourceFaker();
        var jObjectPhoneNumber = phoneNumberResourceFaker.GetJObjectPhoneNumberResource();
        var phoneNumberResource = PhoneNumberResource.FromJson(
            JsonConvert.SerializeObject(jObjectPhoneNumber)
        );

        // Act
        var result = phoneNumberResource.ParseScanResult();

        // Assert
        phoneNumberResource.CallingCountryCode.Should().Be(result.CallingCountryCode);
        phoneNumberResource.CountryCode.Should().Be(result.CountryCode);
        phoneNumberResource.NationalFormat.Should().Be(result.NationalFormat);
        phoneNumberResource.PhoneNumber.ToString().Should().Be(result.PhoneNumber);
        phoneNumberResource.Url.Should().Be(result.Url);
        phoneNumberResource.Valid.Should().Be(result.Valid);
        phoneNumberResource.ValidationErrors.Select(x => x.ToString()).AsEnumerable().Should().BeEquivalentTo(result.ValidationErrors);
    }

    [Fact]
    public void ParseScanResult_ShouldParseResultSuccessfully_ForTwilioCallerNameObject()
    {
        // Arrange
        var phoneNumberResourceFaker = new JObjectPhoneNumberResourceFaker();
        var jObjectPhoneNumber = phoneNumberResourceFaker.GetJObjectPhoneNumberResource();
        var phoneNumberResource = PhoneNumberResource.FromJson(
            JsonConvert.SerializeObject(jObjectPhoneNumber)
        );

        var callerNameObject = (JObject)phoneNumberResource.CallerName;
        var callerName = (string?)callerNameObject?.Property("caller_name");
        var callerType = (string?)callerNameObject?.Property("caller_type");
        var errorCode = (int?)callerNameObject?.Property("error_code");

        // Act
        var result = phoneNumberResource.ParseScanResult();

        // Assert
        result.CallerName.Should().NotBeNull();
        callerName.Should().Be(result.CallerName?.Name);
        callerType.Should().Be(result.CallerName?.CallerType);
        errorCode.Should().Be(result.CallerName?.ErrorCode);
    }

    [Fact]
    public void ParseScanResult_ShouldParseResultSuccessfully_ForTwilioCallForwardingObject()
    {
        // Arrange
        var phoneNumberResourceFaker = new JObjectPhoneNumberResourceFaker();
        var jObjectPhoneNumber = phoneNumberResourceFaker.GetJObjectPhoneNumberResource();
        var phoneNumberResource = PhoneNumberResource.FromJson(
            JsonConvert.SerializeObject(jObjectPhoneNumber)
        );

        var callForwardingObject = (JObject)phoneNumberResource.CallForwarding;
        var callForwardingStatus = (bool?)callForwardingObject?.Property("call_forwarding_status");
        var carrierName = (string?)callForwardingObject?.Property("carrier_name");
        var errorCode = (int?)callForwardingObject?.Property("error_code");
        var mobileCountryCode = (string?)callForwardingObject?.Property("mobile_country_code");
        var mobileNetworkCode = (string?)callForwardingObject?.Property("mobile_network_code");

        // Act
        var result = phoneNumberResource.ParseScanResult();

        // Assert
        result.CallForwarding.Should().NotBeNull();
        callForwardingStatus.Should().Be(result.CallForwarding?.CallForwardingStatus);
        carrierName.Should().Be(result.CallForwarding?.CarrierName);
        errorCode.Should().Be(result.CallForwarding?.ErrorCode);
        mobileCountryCode.Should().Be(result.CallForwarding?.MobileCountryCode);
        mobileNetworkCode.Should().Be(result.CallForwarding?.MobileNetworkCode);
    }

    [Fact]
    public void ParseScanResult_ShouldParseResultSuccessfully_ForTwilioLineTypeIntelligenceObject()
    {
        // Arrange
        var phoneNumberResourceFaker = new JObjectPhoneNumberResourceFaker();
        var jObjectPhoneNumber = phoneNumberResourceFaker.GetJObjectPhoneNumberResource();
        var phoneNumberResource = PhoneNumberResource.FromJson(
            JsonConvert.SerializeObject(jObjectPhoneNumber)
        );

        var lineTypeIntelligenceObject = (JObject)phoneNumberResource.LineTypeIntelligence;
        var carrierName = (string?)lineTypeIntelligenceObject?.Property("carrier_name");
        var errorCode = (int?)lineTypeIntelligenceObject?.Property("error_code");
        var mobileCountryCode = (string?)lineTypeIntelligenceObject?.Property("mobile_country_code");
        var mobileNetworkCode = (string?)lineTypeIntelligenceObject?.Property("mobile_network_code");
        var type = (string?)lineTypeIntelligenceObject?.Property("type");


        // Act
        var result = phoneNumberResource.ParseScanResult();

        // Assert
        result.LineTypeIntelligence.Should().NotBeNull();
        carrierName.Should().Be(result.LineTypeIntelligence?.CarrierName);
        errorCode.Should().Be(result.LineTypeIntelligence?.ErrorCode);
        mobileCountryCode.Should().Be(result.LineTypeIntelligence?.MobileCountryCode);
        mobileNetworkCode.Should().Be(result.LineTypeIntelligence?.MobileNetworkCode);
        type.Should().Be(result.LineTypeIntelligence?.Type);
    }

    [Fact]
    public void ParseScanResult_ShouldParseResultSuccessfully_ForTwilioLineLiveActivityObject()
    {
        // Arrange
        var phoneNumberResourceFaker = new JObjectPhoneNumberResourceFaker();
        var jObjectPhoneNumber = phoneNumberResourceFaker.GetJObjectPhoneNumberResource();
        var phoneNumberResource = PhoneNumberResource.FromJson(
            JsonConvert.SerializeObject(jObjectPhoneNumber)
        );

        var liveActivityObject = (JObject)phoneNumberResource.LiveActivity;
        var connectivity = (string?)liveActivityObject?.Property("connectivity");
        var ported = (bool?)liveActivityObject?.Property("ported");

        var originalCarrierObject = liveActivityObject?.Property("original_carrier");
        var portedCarrierObject = liveActivityObject?.Property("ported_carrier");
        var roamingCarrierObject = liveActivityObject?.Property("roaming_carrier");

        var originalCarrierMobileCountryCode = originalCarrierObject?.Value?.Value<string?>("mobile_country_code");
        var originalCarrierMobileNetworkCode = originalCarrierObject?.Value?.Value<string?>("mobile_network_code");
        var originalCarrierName = originalCarrierObject?.Value?.Value<string?>("name");

        var portedCarrierMobileCountryCode = portedCarrierObject?.Value?.Value<string?>("mobile_country_code");
        var portedCarrierMobileNetworkCode = portedCarrierObject?.Value?.Value<string?>("mobile_network_code");
        var portedCarrierName = portedCarrierObject?.Value?.Value<string?>("name");

        var roamingCarrierCountry = roamingCarrierObject?.Value?.Value<string?>("country");
        var roamingCarrierMobileCountryCode = roamingCarrierObject?.Value?.Value<string?>("mobile_country_code");
        var roamingCarrierMobileNetworkCode = roamingCarrierObject?.Value?.Value<string?>("mobile_network_code");
        var roamingCarrierName = roamingCarrierObject?.Value?.Value<string?>("name");

        // Act
        var result = phoneNumberResource.ParseScanResult();

        // Assert
        result.LiveActivity.Should().NotBeNull();
        connectivity.Should().Be(result.LiveActivity?.Connectivity);
        ported.Should().Be(result.LiveActivity?.Ported);

        result.LiveActivity?.OriginalCarrier.Should().NotBeNull();
        originalCarrierMobileCountryCode.Should().Be(result.LiveActivity?.OriginalCarrier?.MobileCountryCode);
        originalCarrierMobileNetworkCode.Should().Be(result.LiveActivity?.OriginalCarrier?.MobileNetworkCode);
        originalCarrierName.Should().Be(result.LiveActivity?.OriginalCarrier?.Name);

        result.LiveActivity?.PortedCarrier.Should().NotBeNull();
        portedCarrierMobileCountryCode.Should().Be(result.LiveActivity?.PortedCarrier?.MobileCountryCode);
        portedCarrierMobileNetworkCode.Should().Be(result.LiveActivity?.PortedCarrier?.MobileNetworkCode);
        portedCarrierName.Should().Be(result.LiveActivity?.PortedCarrier?.Name);

        result.LiveActivity?.RoamingCarrier.Should().NotBeNull();
        roamingCarrierCountry.Should().Be(result.LiveActivity?.RoamingCarrier?.Country);
        roamingCarrierMobileCountryCode.Should().Be(result.LiveActivity?.RoamingCarrier?.MobileCountryCode);
        roamingCarrierMobileNetworkCode.Should().Be(result.LiveActivity?.RoamingCarrier?.MobileNetworkCode);
        roamingCarrierName.Should().Be(result.LiveActivity?.RoamingCarrier?.Name);
    }

    [Fact]
    public void ParseScanResult_ShouldParseResultSuccessfully_ForTwilioSimSwapObject()
    {
        // Arrange
        var phoneNumberResourceFaker = new JObjectPhoneNumberResourceFaker();
        var jObjectPhoneNumber = phoneNumberResourceFaker.GetJObjectPhoneNumberResource();
        var phoneNumberResource = PhoneNumberResource.FromJson(
            JsonConvert.SerializeObject(jObjectPhoneNumber)
        );

        var simSwapObject = (JObject)phoneNumberResource.SimSwap;
        var carrierName = (string?)simSwapObject?.Property("carrier_name");
        var errorCode = (int?)simSwapObject?.Property("error_code");
        var mobileCountryCode = (string?)simSwapObject?.Property("mobile_country_code");
        var mobileNetworkCode = (string?)simSwapObject?.Property("mobile_network_code");

        var lastSimSwapObject = simSwapObject?.Property("last_sim_swap");
        var dateString = lastSimSwapObject?.Value?.Value<string?>("last_sim_swap_date");
        var lastSimSwapDate = Convert.ToDateTime(dateString);
        var lastSimSwapSwappedInPeriod = lastSimSwapObject?.Value?.Value<bool?>("swapped_in_period");
        var lastSimSwapSwappedPeriod = lastSimSwapObject?.Value?.Value<string?>("swapped_period");

        // Act
        var result = phoneNumberResource.ParseScanResult();

        // Assert
        result.SimSwap.Should().NotBeNull();
        carrierName.Should().Be(result.SimSwap?.CarrierName);
        errorCode.Should().Be(result.SimSwap?.ErrorCode);
        mobileCountryCode.Should().Be(result.SimSwap?.MobileCountryCode);
        mobileNetworkCode.Should().Be(result.SimSwap?.MobileNetworkCode);

        result.SimSwap?.LastSimSwap.Should().NotBeNull();
        lastSimSwapDate.Should().Be(result.SimSwap?.LastSimSwap?.LastSimSwapDate);
        lastSimSwapSwappedInPeriod.Should().Be(result.SimSwap?.LastSimSwap?.SwappedInPeriod);
        lastSimSwapSwappedPeriod.Should().Be(result.SimSwap?.LastSimSwap?.SwappedPeriod);
    }

    [Fact]
    public void ParseScanResult_ShouldParseResultSuccessfully_ForNullObject()
    {
        // Arrange
        var phoneNumberResourceFaker = new JObjectPhoneNumberResourceFaker
        {
            HasCallerName = false,
            HasCallForwarding = false,
            HasLineTypeIntelligence = false,
            HasLiveActivity = false,
            HasSimSwap = false
        };

        var jObjectPhoneNumber = phoneNumberResourceFaker.GetJObjectPhoneNumberResource();

        var phoneNumberResource = PhoneNumberResource.FromJson(
            JsonConvert.SerializeObject(jObjectPhoneNumber)
        );

        // Act
        var result = phoneNumberResource.ParseScanResult();

        // Assert
        result.CallerName.Should().BeNull();
        result.CallForwarding.Should().BeNull();
        result.LineTypeIntelligence.Should().BeNull();
        result.LiveActivity.Should().BeNull();
        result.SimSwap.Should().BeNull();
    }

    [Fact]
    public void ParseScanResult_ShouldParseResultSuccessfully_ForLiveActivityNullObject()
    {
        // Arrange
        var phoneNumberResourceFaker = new JObjectPhoneNumberResourceFaker
        {
            HasOriginalCarrier = false,
            HasPortedCarrier = false,
            HasRoamingCarrier  = false
        };

        var jObjectPhoneNumber = phoneNumberResourceFaker.GetJObjectPhoneNumberResource();

        var phoneNumberResource = PhoneNumberResource.FromJson(
            JsonConvert.SerializeObject(jObjectPhoneNumber)
        );

        // Act
        var result = phoneNumberResource.ParseScanResult();

        // Assert
        result.LiveActivity.Should().NotBeNull();
        result.LiveActivity?.OriginalCarrier.Should().BeNull();
        result.LiveActivity?.PortedCarrier.Should().BeNull();
        result.LiveActivity?.RoamingCarrier.Should().BeNull();
    }

    [Fact]
    public void ParseScanResult_ShouldParseResultSuccessfully_ForSimSwapNullObject()
    {
        // Arrange
        var phoneNumberResourceFaker = new JObjectPhoneNumberResourceFaker
        {
            HasLastSimSwap = false
        };

        var jObjectPhoneNumber = phoneNumberResourceFaker.GetJObjectPhoneNumberResource();

        var phoneNumberResource = PhoneNumberResource.FromJson(
            JsonConvert.SerializeObject(jObjectPhoneNumber)
        );

        // Act
        var result = phoneNumberResource.ParseScanResult();

        // Assert
        result.SimSwap.Should().NotBeNull();
        result.SimSwap?.LastSimSwap.Should().BeNull();
    }
}
