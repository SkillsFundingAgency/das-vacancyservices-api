using System;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureApplicationClosingDateValidator()
        {
            var tomorrow = DateTime.Today.AddDays(1);

            RuleFor(request => request.ApplicationClosingDate)
                .NotEmpty()
                .WithMessage(ErrorMessages.CreateApprenticeship.ApplicationClosingDateEmpty)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationClosingDate)
                .DependentRules(rules => rules.RuleFor(request => request.ApplicationClosingDate)
                    .GreaterThanOrEqualTo(tomorrow)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationClosingDate)
                    .WithMessage(ErrorMessages.CreateApprenticeship.ApplicationClosingDateBeforeTomorrow));
        }
    }
}