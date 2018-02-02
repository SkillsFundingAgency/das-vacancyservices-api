using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const double HoursPerWeekMinimumLength = 16;
        private const double HoursPerWeekMaximumLength = 48;

        private void ConfigureHoursPerWeekValidator()
        {
            RuleFor(request => request.HoursPerWeek)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.HoursPerWeek)
                .DependentRules(rules => rules.RuleFor(request => request.HoursPerWeek)
                    .InclusiveBetween(HoursPerWeekMinimumLength, HoursPerWeekMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.HoursPerWeek));
        }
    }
}