using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureExpectedStartDateValidator()
        {
            RuleFor(request => request.ExpectedStartDate)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedStartDate)
                .DependentRules(rules => rules.RuleFor(request => request.ExpectedStartDate)
                    .GreaterThanOrEqualTo(request => request.ApplicationClosingDate.Date.AddDays(1))
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedStartDate)
                    .WithMessage(ErrorMessages.CreateApprenticeship.ExpectedStartDateBeforeClosingDate));
        }
    }
}