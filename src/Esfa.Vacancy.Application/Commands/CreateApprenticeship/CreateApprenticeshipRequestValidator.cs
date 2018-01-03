using System;
using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;


namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        public const string RegexFreeTextWhiteList = @"^[a-zA-Z0-9\u0080-\uFFA7?$@#()""'!,+\-=_:;.&€£*%\s\/\[\]]+$";

        private const string TitleApprentice = "apprentice";
        private const int TitleMaximumLength = 100;
        private const int ShortDescriptionMaximumLength = 350;

        public CreateApprenticeshipRequestValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .WithErrorCode(TitleIsRequired)
                .DependentRules(rules =>
                {
                    rules.RuleFor(request => request.Title)
                        .MaximumLength(TitleMaximumLength)
                        .WithErrorCode(TitleMaximumFieldLength)
                        .Must(title => title.IndexOf(TitleApprentice, StringComparison.OrdinalIgnoreCase) >= 0)
                        .WithErrorCode(TitleShouldIncludeWordApprentice)
                        .Matches(RegexFreeTextWhiteList)
                        .WithErrorCode(TitleShouldNotIncludeSpecialCharacters);
                });

            RuleFor(request => request.ShortDescription)
                .NotEmpty()
                .WithErrorCode(ShortDescriptionShouldBeSpecified)
                .DependentRules(rules =>
                {
                    rules.RuleFor(request => request.ShortDescription)
                        .MaximumLength(ShortDescriptionMaximumLength)
                        .WithErrorCode(ShortDescriptionMaximumFieldLength)
                        .Matches(RegexFreeTextWhiteList)
                        .WithErrorCode(ShortDescriptionShouldNotIncludeSpecialCharacters);
                });

        }
    }
}