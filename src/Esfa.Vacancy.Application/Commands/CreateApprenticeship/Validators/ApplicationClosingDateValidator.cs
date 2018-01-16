using System;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ApplicationClosingDateValidator()
        {
            var tomorrow = DateTime.Today.AddDays(1);

            RuleFor(request => request.ApplicationClosingDate)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationClosingDateRequired)
                .DependentRules(rules => rules.RuleFor(request => request.ApplicationClosingDate)
                    .GreaterThanOrEqualTo(tomorrow)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationClosingDateBeforeTomorrow)
                    .WithMessage(ErrorMessages.CreateApprenticeship.ApplicationClosingDateBeforeTomorrow));
        }
    }
}