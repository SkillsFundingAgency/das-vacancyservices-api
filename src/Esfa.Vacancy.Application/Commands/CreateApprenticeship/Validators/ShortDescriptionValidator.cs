using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public class ShortDescriptionValidator : AbstractValidator<string>
    {
        private const int ShortDescriptionMaximumLength = 350;

        public ShortDescriptionValidator()
        {
            RuleFor(x => x)
                .MaximumLength(ShortDescriptionMaximumLength)
                .WithErrorCode(ShortDescriptionMaximumFieldLength)
                .MatchesAllowedFreeTextCharacters(ShortDescriptionShouldNotIncludeSpecialCharacters);
                
        }
    }
}
