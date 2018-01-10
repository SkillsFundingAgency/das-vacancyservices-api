using System.Text.RegularExpressions;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public static class Extensions
    {
        private const string RegexFreeTextWhitelist = @"^[a-zA-Z0-9\u0080-\uFFA7?$@#()""'!,+\-=_:;.&€£*%\s\/\[\]]+$";
        private const string RegexHtmlFreeTextWhitelist = @"^[a-zA-Z0-9\u0080-\uFFA7?$@#()""'!,+\-=_:;.&€£*%\s\/<>\[\]]+$";
        private const string RegexScriptsBlacklist = @"<\s*s\s*c\s*r\s*i\s*p\s*t\s*[^>]*\s*[^>]*\s*[^>]*>";
        private const string RegexInputsBlacklist = @"<\s*i\s*n\s*p\s*u\s*t\s*[^>]*\s*[^>]*\s*[^>]*>";
        private const string RegexObjectsBlacklist = @"<\s*o\s*b\s*j\s*e\s*c\s*t\s*[^>]*\s*[^>]*\s*[^>]*>";

        public static IRuleBuilderOptions<string, string> MatchesAllowedFreeTextCharacters(
            this IRuleBuilder<string, string> rule, string errorCode, string errorMessage, string propertyName)
        {
            return rule.Matches(RegexFreeTextWhitelist)
                .WithErrorCode(errorCode)
                .WithName(propertyName)
                .WithMessage(errorMessage);
        }

        public static IRuleBuilderOptions<string, string> MatchesAllowedHtmlFreeTextCharacters(
            this IRuleBuilder<string, string> rule, string whitelistErrorCode, string whitelistErrorMessage, string blacklistErrorCode, string blacklistErrorMessage, string propertyName)
        {
            return rule.Matches(RegexHtmlFreeTextWhitelist)
                .WithErrorCode(whitelistErrorCode)
                .WithName(propertyName)
                .WithMessage(whitelistErrorMessage)

                .Must(CheckHtmlFreeTextBlacklist)
                .WithErrorCode(blacklistErrorCode)
                .WithName(propertyName)
                .WithMessage(blacklistErrorMessage);
        }

        private static bool CheckHtmlFreeTextBlacklist(string text)
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
