using System;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;


namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public class TitleValidator : AbstractValidator<string>
    {
        private const string PropertyName = "Title";
        private const string TitleApprentice = "apprentice";
        private const int TitleMaximumLength = 100;

        public TitleValidator()
        {
            RuleFor(x => x)
                .MaximumLength(TitleMaximumLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TitleMaximumFieldLength)
                .WithName(PropertyName)

                .Must(title => title.IndexOf(TitleApprentice, StringComparison.OrdinalIgnoreCase) >= 0)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TitleShouldIncludeWordApprentice)
                .WithName(PropertyName)
                .WithMessage(ErrorMessages.CreateApprenticeship.TitleShouldIncludeWordApprentice)

                .MatchesAllowedFreeTextCharacters(ErrorCodes.CreateApprenticeship.TitleShouldNotIncludeSpecialCharacters, ErrorMessages.CreateApprenticeship.TitleShouldNotIncludeSpecialCharacters, PropertyName);
        }
    }
}
