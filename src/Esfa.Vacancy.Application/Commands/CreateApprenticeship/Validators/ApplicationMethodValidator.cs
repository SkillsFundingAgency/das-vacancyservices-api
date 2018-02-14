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
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationMethod)
                .DependentRules(rules => rules.RuleFor(request => request.ApplicationMethod)
                                              .IsInEnum()
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationMethod));

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

                RuleFor(request => request.ExternalApplicationUrl)
                    .Empty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExternalApplicationUrl)
                    .WithMessage(ErrorMessages.CreateApprenticeship.ExternalApplicationValuesNotToBeSpecified);

                RuleFor(request => request.ExternalApplicationInstructions)
                    .Empty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExternalApplicationInstructions)
                    .WithMessage(ErrorMessages.CreateApprenticeship.ExternalApplicationValuesNotToBeSpecified);
            });

        }
    }
}
