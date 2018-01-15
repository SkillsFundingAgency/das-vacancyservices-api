using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void WorkingWeekValidator()
        {
            RuleFor(request => request.WorkingWeek)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WorkingWeekRequired)
                .DependentRules(rules => rules.RuleFor(request => request.WorkingWeek)
                    .MaximumLength(250)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WorkingWeekLengthGreaterThan250));
        }
    }
}