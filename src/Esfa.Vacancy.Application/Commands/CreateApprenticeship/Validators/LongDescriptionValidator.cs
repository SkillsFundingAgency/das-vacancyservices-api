using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void LongDescriptionValidator()
        {
            RuleFor(request => request.LongDescription)
                .NotEmpty()
                .WithErrorCode(LongDescriptionIsRequired)
                .DependentRules(rules => rules.RuleFor(request => request.LongDescription)
                    .MatchesAllowedHtmlFreeTextCharacters(LongDescriptionShouldNotIncludeSpecialCharacters,
                    LongDescriptionShouldNotIncludeBlacklistedHtmlElements));
        }
    }
}
