using System;
using System.Text.RegularExpressions;
using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;


namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        public const string RegexFreeTextWhiteList = @"^[a-zA-Z0-9\u0080-\uFFA7?$@#()""'!,+\-=_:;.&€£*%\s\/\[\]]+$";
        public const string RegexHtmlFreeTextWhiteList = @"^[a-zA-Z0-9\u0080-\uFFA7?$@#()""'!,+\-=_:;.&€£*%\s\/<>\[\]]+$";
        public const string RegexScriptsBlacklist = @"<\s*s\s*c\s*r\s*i\s*p\s*t\s*[^>]*\s*[^>]*\s*[^>]*>";
        public const string RegexInputsBlacklist = @"<\s*i\s*n\s*p\s*u\s*t\s*[^>]*\s*[^>]*\s*[^>]*>";
        public const string RegexObjectsBlacklist = @"<\s*o\s*b\s*j\s*e\s*c\s*t\s*[^>]*\s*[^>]*\s*[^>]*>";

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

            RuleFor(request => request.LongDescription)
                .NotEmpty()
                .WithErrorCode(LongDescriptionShouldBeSpecified)
                .DependentRules(rules =>
                {
                    rules.RuleFor(request => request.LongDescription)
                        .Matches(RegexHtmlFreeTextWhiteList)
                        .WithErrorCode(LongDescriptionShouldNotIncludeSpecialCharacters)
                        .Must(CheckHtmlFreeTextBlacklist)
                        .WithErrorCode(LongDescriptionShouldNotIncludeBlacklistedHtmlElements);
                });

        }

        public static bool CheckHtmlFreeTextBlacklist(string text)
        {
            if (Regex.IsMatch(text, RegexScriptsBlacklist) ||
                Regex.IsMatch(text, RegexInputsBlacklist) ||
                Regex.IsMatch(text, RegexObjectsBlacklist))
            {
                return false;
            }
            return true;
        }
    }
}