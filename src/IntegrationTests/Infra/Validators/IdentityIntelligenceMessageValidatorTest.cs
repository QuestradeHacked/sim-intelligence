using FluentValidation.TestHelper;
using FluentAssertions;
using Infra.Validators;
using Xunit;

namespace IntegrationTests.Infra.Validators;

public class IdentityIntelligenceMessageValidatorTest
{
    private readonly IdentityIntelligenceMessageValidator _validator;

    public IdentityIntelligenceMessageValidatorTest()
    {
        _validator = new IdentityIntelligenceMessageValidator();
    }

    [Fact]
    public async Task ValidateAsync_WhenReceiveAValidMessage_ShouldBeSuccess()
    {
        //Arrange
        var message = Faker.IdentityIntelligenceFaker.GetValidIdentityIntelligenceMessage();

        //Act
        var result = await _validator.TestValidateAsync(message);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WhenReceiveAMessageWithoutPhoneNumber_ShouldFail()
    {
        //Arrange
        var message = Faker.IdentityIntelligenceFaker.GetInvalidIdentityIntelligenceMessageWithoutPhoneNumber();

        //Act
        var result = await _validator.TestValidateAsync(message);

        //Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateAsync_WhenReceiveAMessageWithoutUserId_ShouldFail()
    {
        //Arrange
        var message = Faker.IdentityIntelligenceFaker.GetInvalidIdentityIntelligenceMessageWithoutUserId();

        //Act
        var result = await _validator.TestValidateAsync(message);

        //Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateAsync_WhenReceiveAMessageWithoutIdMatchFields_ShouldFail()
    {
        //Arrange
        var message = Faker.IdentityIntelligenceFaker.GetInvalidIdentityIntelligenceMessageWithoutIdMatchFields();

        //Act
        var result = await _validator.TestValidateAsync(message);

        //Assert
        result.IsValid.Should().BeFalse();
    }
}
