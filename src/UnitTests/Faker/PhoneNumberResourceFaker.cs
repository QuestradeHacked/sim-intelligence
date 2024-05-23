using Newtonsoft.Json.Linq;

namespace UnitTests.Faker;

public class JObjectPhoneNumberResourceFaker
{
    private readonly Bogus.Faker _faker = new();

    public string? CallingCountryCode { get; set; }

    public string? CountryCode { get; set; }

    public bool HasCallerName { get; set; } = true;

    public bool HasCallForwarding { get; set; } = true;

    public bool HasLineTypeIntelligence { get; set; } = true;

    public bool HasLiveActivity { get; set; } = true;

    public bool HasSimSwap { get; set; } = true;

    public bool HasLastSimSwap { get; set; } = true;

    public bool HasPortedCarrier { get; set; } = true;

    public bool HasRoamingCarrier { get; set; } = true;

    public bool HasOriginalCarrier { get; set; } = true;

    public string? NationalFormat { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Url { get; set; }

    public bool? Valid { get; private set; }

    public string[]? ValidationErrors { get; set; }

    public JObjectPhoneNumberResourceFaker()
    {
        var phoneNumberFake = _faker.Phone.PhoneNumber("+#############");

        CallingCountryCode = phoneNumberFake.Substring(1, 2);
        CountryCode = _faker.Address.CountryCode();
        NationalFormat = $"({phoneNumberFake.Substring(3, 2)}) {phoneNumberFake.Substring(5, 4)}-{phoneNumberFake.Substring(10, 4)}";
        PhoneNumber = phoneNumberFake;
        Url = _faker.Internet.Url();
        Valid = true;
        ValidationErrors = new string[] { };
    }

    public JObject? GenerateCarrier()
    {
        var carrier = JObject.FromObject(new
        {
            mobile_country_code = _faker.Random.Number(999).ToString().PadLeft(3, '0'),
            mobile_network_code = _faker.Random.Number(999).ToString().PadLeft(2, '0'),
            name = _faker.Company.CompanyName(0),
        });

        return carrier;
    }

    public JObject? GenerateCallerName()
    {
        if (!HasCallerName)
        {
            return null;
        }

        var haveData = _faker.Random.Bool();

        var callerName = JObject.FromObject(new
        {
            caller_name = haveData ? _faker.Person.FullName : null,
            caller_type = haveData ? _faker.Lorem.Word() : null,
            error_code = (string?)null
        });

        return callerName;
    }

    public JObject? GenerateCallForwarding()
    {
        if (!HasCallForwarding)
        {
            return null;
        }

        var carrier = GenerateCarrier();
        var callForwarding = new
        {
            call_forwarding_status = _faker.Random.Bool().ToString().ToLower(),
            carrier_name = carrier?.GetValue("name"),
            error_code = (string?)null,
            mobile_country_code = carrier?.GetValue("mobile_country_code"),
            mobile_network_code = carrier?.GetValue("mobile_network_code"),
        };

        return JObject.FromObject(callForwarding);
    }

    public JObject? GenerateLastSimSwap()
    {
        if (!HasLastSimSwap)
        {
            return null;
        }

        var lastSimSwap = JObject.FromObject(new
        {
            last_sim_swap_date = _faker.Date.Past(2).ToString("T"),
            swapped_in_period = _faker.Random.Bool().ToString().ToLower(),
            swapped_period = _faker.Phone.PhoneNumber("PT#####H##M##S")
        });

        return lastSimSwap;
    }

    public JObject? GenerateLineTypeIntelligence()
    {
        if (!HasLineTypeIntelligence)
        {
            return null;
        }

        var carrier = GenerateCarrier();
        var lineTypeIntelligence = JObject.FromObject(new
        {
            carrier_name = carrier?.GetValue("name"),
            error_code = (string?)null,
            mobile_country_code = carrier?.GetValue("mobile_country_code"),
            mobile_network_code = carrier?.GetValue("mobile_network_code"),
            type = _faker.Lorem.Word()
        });

        return lineTypeIntelligence;
    }

    public JObject? GenerateLiveActivity()
    {
        if (!HasLiveActivity)
        {
            return null;
        }

        return JObject.FromObject(new
        {
            connectivity = _faker.Lorem.Word(),
            ported = HasPortedCarrier,
            ported_carrier = HasPortedCarrier ? GenerateCarrier() : null,
            roaming_carrier = GenerateRoamingCarrier(),
            original_carrier = HasOriginalCarrier ? GenerateCarrier() : null,
            error_code = (string?)null
        });
    }

    public JObject? GenerateRoamingCarrier()
    {
        if (!HasRoamingCarrier )
        {
            return null;
        }

        var carrier = GenerateCarrier();
        carrier?.Add("country", _faker.Address.CountryCode());

        return carrier;
    }

    public JObject? GenerateSwimSwap()
    {
        if (!HasSimSwap)
        {
            return null;
        }

        var carrier = GenerateCarrier();

        var simSwap = JObject.FromObject(new
        {
            carrier_name = carrier?.GetValue("name"),
            error_code = (string?)null,
            mobile_country_code = carrier?.GetValue("mobile_country_code"),
            mobile_network_code = carrier?.GetValue("mobile_network_code"),
            last_sim_swap = GenerateLastSimSwap()
        });

        return simSwap;
    }

    public JObject GetJObjectPhoneNumberResource()
    {
        var phoneNumberResourceJObject = JObject.FromObject(new
        {
            calling_country_code = CallingCountryCode,
            country_code = CountryCode,
            national_format = NationalFormat,
            phone_number = PhoneNumber,
            url = Url,
            valid = Valid,
            validation_errors = ValidationErrors
        });
        phoneNumberResourceJObject.Add("caller_name", GenerateCallerName());
        phoneNumberResourceJObject.Add("call_forwarding", GenerateCallForwarding());
        phoneNumberResourceJObject.Add("live_activity", GenerateLiveActivity());
        phoneNumberResourceJObject.Add("line_type_intelligence", GenerateLineTypeIntelligence());
        phoneNumberResourceJObject.Add("sim_swap", GenerateSwimSwap());

        return phoneNumberResourceJObject;
    }
}
