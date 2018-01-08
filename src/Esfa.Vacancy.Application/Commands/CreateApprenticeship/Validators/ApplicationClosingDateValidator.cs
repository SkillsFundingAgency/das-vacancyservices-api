using System;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public class ApplicationClosingDateValidator : AbstractValidator<DateTime>
    {
        public ApplicationClosingDateValidator()
        {
            RuleFor(closingDate => closingDate)
                .GreaterThanOrEqualTo(DateTime.Today.AddDays(1));
        }
    }
}