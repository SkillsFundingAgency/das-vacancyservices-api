using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void LongDescriptionValidator()
        {
            RuleFor(request => request.LongDescription)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.LongDescriptionIsRequired)
                .DependentRules(rules => rules.RuleFor(request => request.LongDescription)
                    .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes.CreateApprenticeship.LongDescriptionShouldNotIncludeSpecialCharacters,
                    ErrorCodes.CreateApprenticeship.LongDescriptionShouldNotIncludeBlacklistedHtmlElements));
        }
    }
}
