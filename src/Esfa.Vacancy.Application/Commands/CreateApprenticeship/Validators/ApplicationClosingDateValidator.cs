using System;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public class ApplicationClosingDateValidator : AbstractValidator<DateTime>
    {
        public ApplicationClosingDateValidator()
        {
            var tomorrow = DateTime.Today.AddDays(1);

            RuleFor(closingDate => closingDate)
                .GreaterThanOrEqualTo(tomorrow)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationClosingDateBeforeTomorrow)
                .WithMessage(ErrorMessages.CreateApprenticeship.ApplicationClosingDateBeforeTomorrow);
        }
    }
}