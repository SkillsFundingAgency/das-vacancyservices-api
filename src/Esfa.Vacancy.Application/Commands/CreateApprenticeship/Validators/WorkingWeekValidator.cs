using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const int WorkingWeekMaximumLength = 250;

        private void ConfigureWorkingWeekValidator()
        {
            RuleFor(request => request.WorkingWeek)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WorkingWeek)
                .DependentRules(rules => rules.RuleFor(request => request.WorkingWeek)
                    .MaximumLength(WorkingWeekMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WorkingWeek)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WorkingWeek));
        }
    }
}