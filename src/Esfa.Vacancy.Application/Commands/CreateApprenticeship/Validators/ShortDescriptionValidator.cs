using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const int ShortDescriptionMaximumLength = 350;

        private void ShortDescriptionValidator()
        {
            RuleFor(request => request.ShortDescription)
                .NotEmpty()
                .WithErrorCode(ShortDescriptionIsRequired)
                .DependentRules(rules => rules.RuleFor(request => request.ShortDescription)
                    .MaximumLength(ShortDescriptionMaximumLength)
                    .WithErrorCode(ShortDescriptionMaximumFieldLength)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ShortDescriptionShouldNotIncludeSpecialCharacters));
        }
    }
}
