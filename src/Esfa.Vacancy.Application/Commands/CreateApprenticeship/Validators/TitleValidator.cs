using System;
using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public class TitleValidator : AbstractValidator<string>
    {
        private const string TitleApprentice = "apprentice";
        private const int TitleMaximumLength = 100;

        public TitleValidator()
        {
            RuleFor(x => x)
                .MaximumLength(TitleMaximumLength)
                .WithErrorCode(TitleMaximumFieldLength)
                .Must(title => title.IndexOf(TitleApprentice, StringComparison.OrdinalIgnoreCase) >= 0)
                .WithErrorCode(TitleShouldIncludeWordApprentice)
                .MatchesAllowedFreeTextCharacters(TitleShouldNotIncludeSpecialCharacters);
               
        }
    }
}
