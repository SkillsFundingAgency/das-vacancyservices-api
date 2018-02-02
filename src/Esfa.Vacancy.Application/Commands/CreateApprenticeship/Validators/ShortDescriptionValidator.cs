using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const int ShortDescriptionMaximumLength = 350;

        private void ConfigureShortDescriptionValidator()
        {
            RuleFor(request => request.ShortDescription)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ShortDescription)
                .DependentRules(rules => rules.RuleFor(request => request.ShortDescription)
                    .MaximumLength(ShortDescriptionMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ShortDescription)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ShortDescription));
        }
    }
}
