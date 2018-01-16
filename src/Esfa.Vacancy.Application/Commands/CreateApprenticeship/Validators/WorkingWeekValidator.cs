using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const int WorkingWeekMaximumLength = 250;

        private void WorkingWeekValidator()
        {
            RuleFor(request => request.WorkingWeek)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WorkingWeekRequired)
                .DependentRules(rules => rules.RuleFor(request => request.WorkingWeek)
                    .MaximumLength(WorkingWeekMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WorkingWeekLengthGreaterThan250)
                    .MatchesAllowedFreeTextCharacters(
                        ErrorCodes.CreateApprenticeship.WorkingWeekShouldNotIncludeSpecialCharacters, 
                        nameof(CreateApprenticeshipRequest.WorkingWeek)));
        }
    }
}