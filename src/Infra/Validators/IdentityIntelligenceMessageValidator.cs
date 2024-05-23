using FluentValidation;
using Infra.Models.Messages;

namespace Infra.Validators;

public class IdentityIntelligenceMessageValidator : AbstractValidator<IdentityIntelligenceMessage>
{
    public IdentityIntelligenceMessageValidator()
    {
        RuleFor(x => x.Phone)
            .NotNull()
            .WithMessage("A message with empty phone number was received.");

        RuleFor(x => x)
            .Must(EnsureAllRequiredParameterAreProvided)
            .WithMessage("A message with empty profile id and crm user id was received.");

        RuleFor(x => x)
            .Must(EnsureAtLeastOneParameterIsFilledIn)
            .WithMessage("At least one of the fields must be provided.");
    }

    private static bool EnsureAllRequiredParameterAreProvided(IdentityIntelligenceMessage message)
    {
        return !string.IsNullOrEmpty(message.ProfileId) || !string.IsNullOrEmpty(message.CrmUserId);
    }

    private static bool EnsureAtLeastOneParameterIsFilledIn(IdentityIntelligenceMessage message)
    {
        return !string.IsNullOrEmpty(message.FirstName) ||
               !string.IsNullOrEmpty(message.LastName) ||
               !string.IsNullOrEmpty(message.AddressLine1) ||
               !string.IsNullOrEmpty(message.NationalId) ||
               !string.IsNullOrEmpty(message.DateOfBirth) ||
               !string.IsNullOrEmpty(message.City);
    }
}
