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
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageType)
                .DependentRules(rules => rules.RuleFor(request => request.WageType)
                    .IsInEnum()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageType));

            When(request => request.WageType == WageType.Custom, () =>
            {
                RuleFor(request => request.MinWage)
                    .NotNull()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .NotNull()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.WageTypeReason)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);
            });

            When(request => request.WageType == WageType.NationalMinimumWage, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.WageTypeReason)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);
            });

            When(request => request.WageType == WageType.ApprenticeshipMinimumWage, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.WageTypeReason)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);

                RuleFor(request => request.WageUnit)
                    .Equal(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);
            });

            When(request => request.WageType == WageType.Unwaged, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.WageTypeReason)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);
            });

            When(request => request.WageType == WageType.CompetitiveSalary, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.WageTypeReason)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);
            });

            When(request => request.WageType == WageType.ToBeSpecified, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.WageTypeReason)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);
            });

            RuleFor(request => request.WageTypeReason)
                .MaximumLength(240)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason)
                .MatchesAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);
        }

        private bool BeEmptyWageUnit(WageUnit wageUnit)
        {
            return false;
        }
    }
}