using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureWageTypeValidator()
        {
            RuleFor(request => request.WageType)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeError)
                .DependentRules(rules => rules.RuleFor(request => request.WageType)
                    .IsInEnum()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeError));

            When(request => request.WageType == WageType.ApprenticeshipMinimumWage, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWageError);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWageError);
            });
        }
    }
}