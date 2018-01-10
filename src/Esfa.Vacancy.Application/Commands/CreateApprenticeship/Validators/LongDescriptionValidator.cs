using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public class LongDescriptionValidator : AbstractValidator<string>
    {
        public const string PropertyName = "LongDescription";

        public LongDescriptionValidator()
        {
            RuleFor(x => x).MatchesAllowedHtmlFreeTextCharacters(ErrorCodes.CreateApprenticeship.LongDescriptionShouldNotIncludeSpecialCharacters,
                ErrorMessages.CreateApprenticeship.LongDescriptionShouldNotIncludeSpecialCharacters,
                ErrorCodes.CreateApprenticeship.LongDescriptionShouldNotIncludeBlacklistedHtmlElements,
                ErrorMessages.CreateApprenticeship.LongDescriptionShouldNotIncludeBlacklistedHtmlElements,
                PropertyName);
        }
    }
}
