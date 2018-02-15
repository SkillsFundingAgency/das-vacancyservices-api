using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const int MinimumNumberOfWeeks = 52;
        private const int MinimumNumberOfMonths = 12;
        private const int MinimumNumberOfYears = 1;

        private void ConfigureExpectedDurationValidator()
        {
            RuleFor(request => request.DurationType)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.DurationType)
                .DependentRules(rules => rules.RuleFor(request => request.DurationType)
                                              .IsInEnum()
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DurationType));

            When(request => request.DurationType == DurationType.Weeks, () =>
            {
                RuleFor(request => request.ExpectedDuration)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedDuration)
                    .GreaterThanOrEqualTo(MinimumNumberOfWeeks)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedDuration);
            });

            When(request => request.DurationType == DurationType.Months, () =>
            {
                RuleFor(request => request.ExpectedDuration)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedDuration)
                    .GreaterThanOrEqualTo(MinimumNumberOfMonths)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedDuration);
            });

            When(request => request.DurationType == DurationType.Years, () =>
              {
                  RuleFor(request => request.ExpectedDuration)
                      .NotEmpty()
                      .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedDuration)
                      .GreaterThanOrEqualTo(MinimumNumberOfYears)
                      .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedDuration);
              });
        }
    }
}
