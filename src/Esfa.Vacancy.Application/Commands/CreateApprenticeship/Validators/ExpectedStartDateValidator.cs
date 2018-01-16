using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ExpectedStartDateValidator()
        {
            RuleFor(request => request.ExpectedStartDate)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedStartDateRequired)
                .DependentRules(rules => rules.RuleFor(request => request.ExpectedStartDate)
                    .GreaterThanOrEqualTo(request => request.ApplicationClosingDate.Date.AddDays(1))
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedStartDateBeforeClosingDate)
                    .WithMessage(ErrorMessages.CreateApprenticeship.ExpectedStartDateBeforeClosingDate));
        }
    }
}