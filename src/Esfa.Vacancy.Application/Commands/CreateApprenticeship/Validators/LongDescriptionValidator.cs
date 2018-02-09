using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureLongDescriptionValidator()
        {
            RuleFor(request => request.LongDescription)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.LongDescription)
                .DependentRules(rules => rules.RuleFor(request => request.LongDescription)
                    .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes.CreateApprenticeship.LongDescription));
        }
    }
}
