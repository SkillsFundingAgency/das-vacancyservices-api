using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public class LongDescriptionValidator : AbstractValidator<string>
    {
        public LongDescriptionValidator()
        {
            RuleFor(x => x)
                .MatchesAllowedHtmlFreeTextCharacters(LongDescriptionShouldNotIncludeSpecialCharacters, LongDescriptionShouldNotIncludeBlacklistedHtmlElements);
        }
    }
}
