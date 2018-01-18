using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const int ShortDescriptionMaximumLength = 350;

        private void ShortDescriptionValidator()
        {
            RuleFor(request => request.ShortDescription)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ShortDescriptionIsRequired)
                .DependentRules(rules => rules.RuleFor(request => request.ShortDescription)
                    .MaximumLength(ShortDescriptionMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ShortDescriptionMaximumFieldLength)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ShortDescriptionShouldNotIncludeSpecialCharacters));
        }
    }
}
