using System;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const string TitleApprentice = "apprentice";
        private const int TitleMaximumLength = 100;

        private void ConfigureTitleValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TitleIsRequired)

                .DependentRules(rules => rules.RuleFor(request => request.Title)
                    .MaximumLength(TitleMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.TitleMaximumFieldLength)

                    .Must(title => title.IndexOf(TitleApprentice, StringComparison.OrdinalIgnoreCase) >= 0)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.TitleShouldIncludeWordApprentice)
                    .WithMessage(ErrorMessages.CreateApprenticeship.TitleShouldIncludeWordApprentice)

                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.TitleShouldNotIncludeSpecialCharacters));
        }
    }
}
