using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public class ShortDescriptionValidator : AbstractValidator<string>
    {
        private const string PropertyName = "ShortDescription";
        private const int ShortDescriptionMaximumLength = 350;

        public ShortDescriptionValidator()
        {
            RuleFor(x => x)
                .MaximumLength(ShortDescriptionMaximumLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ShortDescriptionMaximumFieldLength)
                .WithName(PropertyName)

                .MatchesAllowedFreeTextCharacters(ErrorCodes.CreateApprenticeship.ShortDescriptionShouldNotIncludeSpecialCharacters,
                    ErrorMessages.CreateApprenticeship.ShortDescriptionShouldNotIncludeSpecialCharacters,
                    PropertyName);

        }
    }
}
