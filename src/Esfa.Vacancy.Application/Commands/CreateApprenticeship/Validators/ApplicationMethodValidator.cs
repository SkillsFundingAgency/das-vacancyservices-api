using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const int SupplementaryQuestionMaximumLength = 4000;

        private void ConfigureApplicationMethodValidator()
        {
            RuleFor(request => request.ApplicationMethod)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .IsInEnum()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationMethod)
                .Must(request => request == ApplicationMethod.Online)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationMethod)
                .WithMessage("Invalid Application Method");

            When(request => request.ApplicationMethod == ApplicationMethod.Online, () =>
            {
                RuleFor(request => request.SupplementaryQuestion1)
                    .MaximumLength(SupplementaryQuestionMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion1)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion1);

                RuleFor(request => request.SupplementaryQuestion2)
                    .MaximumLength(SupplementaryQuestionMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion2)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion2);
            });
        }
    }
}
