using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const double HoursPerWeekMinimumLength = 16;
        private const double HoursPerWeekMaximumLength = 48;

        private void HoursPerWeekValidator()
        {
            RuleFor(request => request.HoursPerWeek)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.HoursPerWeekRequired)
                .DependentRules(rules => rules.RuleFor(request => request.HoursPerWeek)
                    .InclusiveBetween(HoursPerWeekMinimumLength, HoursPerWeekMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.HoursPerWeekOutsideRange));
        }
    }
}