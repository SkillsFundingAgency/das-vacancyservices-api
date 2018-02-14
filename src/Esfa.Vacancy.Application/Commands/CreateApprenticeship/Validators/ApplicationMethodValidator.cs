using System;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const int SupplementaryQuestionMaximumLength = 4000;
        private const int ExternalApplicationInstructionMaximumLength = 4000;

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

            When(request => request.ApplicationMethod == ApplicationMethod.Offline, () =>
            {
                RuleFor(request => request.ExternalApplicationUrl)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExternalApplicationUrl)
                    .DependentRules(rules => rules.RuleFor(request => request.ExternalApplicationUrl)
                                                  .Must(BeValidWebUrl)
                                                  .WithErrorCode(ErrorCodes.CreateApprenticeship.ExternalApplicationUrl)
                                                  .WithMessage(ErrorMessages.CreateApprenticeship.ExternalApplicationUrlInvalid));

                RuleFor(request => request.ExternalApplicationInstructions)
                    .MaximumLength(ExternalApplicationInstructionMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExternalApplicationInstructions)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExternalApplicationInstructions);

                RuleFor(request => request.SupplementaryQuestion1)
                    .Empty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion1)
                    .WithMessage(ErrorMessages.CreateApprenticeship.SupplementaryQuestionNotToBeSpecified);

                RuleFor(request => request.SupplementaryQuestion2)
                    .Empty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion2)
                    .WithMessage(ErrorMessages.CreateApprenticeship.SupplementaryQuestionNotToBeSpecified);
            });
        }

        private static bool BeValidWebUrl(string arg)
        {
            return Uri.TryCreate(arg, UriKind.Absolute, out Uri result) 
                   && (result.Scheme.Equals(Uri.UriSchemeHttp) || result.Scheme.Equals(Uri.UriSchemeHttps));
        }
    }
}
