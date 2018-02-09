using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureApplicationMethodValidator()
        {
            RuleFor(request => request.ApplicationMethod)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationMethod)
                .DependentRules(rules => rules.RuleFor(request => request.ApplicationMethod)
                                              .IsInEnum()
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationMethod)
                                              .Must(request => request == ApplicationMethod.Online)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationMethod)
                                              .WithMessage("Invalid Application Method"));

            When(request => request.ApplicationMethod == ApplicationMethod.Online, () =>
            {
                RuleFor(request => request.SupplementaryQuestion1)
                    .MaximumLength(4000).WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion1)
                    .MatchesAllowedFreeTextCharacters().WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion1);

                RuleFor(request => request.SupplementaryQuestion2)
                    .MaximumLength(4000).WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion2)
                    .MatchesAllowedFreeTextCharacters().WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion2);
            });
        }
    }
}
