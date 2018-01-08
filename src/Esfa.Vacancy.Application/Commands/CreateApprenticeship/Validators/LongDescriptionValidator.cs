using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public class LongDescriptionValidator : AbstractValidator<string>
    {
        public const string PropertyName = "LongDescription";

        public LongDescriptionValidator()
        {
            RuleFor(x => x)
                .MatchesAllowedHtmlFreeTextCharacters(LongDescriptionShouldNotIncludeSpecialCharacters, LongDescriptionShouldNotIncludeBlacklistedHtmlElements, PropertyName);
        }
    }
}
