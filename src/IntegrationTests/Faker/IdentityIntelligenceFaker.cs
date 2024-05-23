using Infra.Config.Twilio;
using Infra.Models.Messages;

namespace IntegrationTests.Faker;

public static class IdentityIntelligenceFaker
{
    public static TwilioConfiguration GetTwilioConfigurationFake()
    {
        var faker = new Bogus.Faker();

        var generatedTwilioConfiguration = new TwilioConfiguration
        {
            AccountSid = faker.Random.Guid().ToString(),
            ApiKey = faker.Random.Uuid().ToString(),
            ApiKeySecret = faker.Random.Uuid().ToString(),
            Enable = true,
            Fields = "caller_name,line_type_intelligence,sim_swap"
        };

        return generatedTwilioConfiguration;
    }

    public static IdentityIntelligenceMessage GetIdentityIntelligenceMessageFake()
    {
        var faker = new Bogus.Faker();

        var generatedIdentityIntelligenceMessage = new IdentityIntelligenceMessage
        {
            CrmUserId = faker.Random.Number().ToString(),
            EnterpriseProfileId = faker.Random.Guid().ToString(),
            Phone = faker.Phone.PhoneNumber("+###########"),
            ProfileId = faker.Random.Guid().ToString(),
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName
        };

        return generatedIdentityIntelligenceMessage;
    }

    public static IdentityIntelligenceMessage GetValidIdentityIntelligenceMessage()
    {
        var faker = new Bogus.Faker();

        var generatedIdentityIntelligenceMessage = new IdentityIntelligenceMessage
        {
            CrmUserId = faker.Random.Number().ToString(),
            EnterpriseProfileId = faker.Random.Guid().ToString(),
            Phone = faker.Phone.PhoneNumber("+###########"),
            ProfileId = faker.Random.Guid().ToString(),
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName
        };

        return generatedIdentityIntelligenceMessage;
    }
    public static IdentityIntelligenceMessage GetInvalidIdentityIntelligenceMessageWithoutPhoneNumber()
    {
        var faker = new Bogus.Faker();

        var generatedIdentityIntelligenceMessage = new IdentityIntelligenceMessage
        {
            CrmUserId = faker.Random.Number().ToString(),
            EnterpriseProfileId = faker.Random.Guid().ToString(),
            ProfileId = faker.Random.Guid().ToString(),
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName
        };

        return generatedIdentityIntelligenceMessage;
    }

    public static IdentityIntelligenceMessage GetInvalidIdentityIntelligenceMessageWithoutUserId()
    {
        var faker = new Bogus.Faker();

        var generatedIdentityIntelligenceMessage = new IdentityIntelligenceMessage
        {
            Phone = faker.Phone.PhoneNumber("+###########"),
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName
        };

        return generatedIdentityIntelligenceMessage;
    }

    public static IdentityIntelligenceMessage GetInvalidIdentityIntelligenceMessageWithoutIdMatchFields()
    {
        var faker = new Bogus.Faker();

        var generatedIdentityIntelligenceMessage = new IdentityIntelligenceMessage
        {
            CrmUserId = faker.Random.Number().ToString(),
            EnterpriseProfileId = faker.Random.Guid().ToString(),
            Phone = faker.Phone.PhoneNumber("+###########"),
            ProfileId = faker.Random.Guid().ToString()
        };

        return generatedIdentityIntelligenceMessage;
    }
}
