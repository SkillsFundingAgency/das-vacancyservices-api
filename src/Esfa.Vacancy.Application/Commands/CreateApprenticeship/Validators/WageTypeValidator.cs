using System;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureWageTypeValidator()
        {
            const int wageTypeReasonMaxLength = 240;

            RuleFor(request => request.WageType)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageType)
                .DependentRules(rules => rules.RuleFor(request => request.WageType)
                    .IsInEnum()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageType));

            RuleFor(request => request.WageUnit)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit)
                .DependentRules(rules => rules.RuleFor(request => request.WageUnit)
                    .IsInEnum()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit));

            When(request => request.WageType == WageType.Custom, () =>
            {
                RuleFor(request => request.MinWage)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage)
                    .MustBeAMonetaryValue()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                When(request => request.MaxWage.HasValue, () =>
                {
                    RuleFor(request => request.MaxWage)
                        .MustBeAMonetaryValue()
                        .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage)
                        .GreaterThanOrEqualTo(request => request.MinWage)
                        .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage)
                        .WithMessage(ErrorMessages.CreateApprenticeship.MaxWageCantBeLessThanMinWage);
                });

                RuleFor(request => request.WageTypeReason)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);

                RuleFor(request => request.WageUnit)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEqual(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit)
                    .MustAsync(BeGreaterThanOrEqualToApprenticeshipMinimumWage)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage)
                    .WithMessage(ErrorMessages.CreateApprenticeship.MinWageIsBelowApprenticeMinimumWage);
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

                RuleFor(request => request.WageUnit)
                    .Equal(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);
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

                RuleFor(request => request.WageUnit)
                    .Equal(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);
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

                RuleFor(request => request.WageUnit)
                    .Equal(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);
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

                RuleFor(request => request.WageUnit)
                    .Equal(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);
            });

            RuleFor(request => request.WageTypeReason)
                .MaximumLength(wageTypeReasonMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason)
                .MatchesAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);
        }

        private async Task<bool> BeGreaterThanOrEqualToApprenticeshipMinimumWage(CreateApprenticeshipRequest request, WageUnit wageUnit, CancellationToken cancellationToken)
        {
            try
            {
                var allowedMinimumWage = await _minimumWageSelector.SelectHourlyRateAsync(request.ExpectedStartDate);
                var attemptedMinimumWage = _minimumWageCalculator.CalculateMinimumWage(request);

                return attemptedMinimumWage >= allowedMinimumWage;
            }
            catch (ArgumentOutOfRangeException outOfRangeException)
            {
                _logger.Debug(outOfRangeException.Message);
                return false;
            }
        }
    }
}