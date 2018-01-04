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
            this IRuleBuilder<string, string> rule, string errorCode)
        {
            return rule.Matches(RegexFreeTextWhitelist)
                .WithErrorCode(errorCode);
        }

        public static IRuleBuilderOptions<string, string> MatchesAllowedHtmlFreeTextCharacters(
            this IRuleBuilder<string, string> rule, string whitelistErrorCode, string blacklistErrorCode)
        {
            return rule.Matches(RegexHtmlFreeTextWhitelist)
                .WithErrorCode(whitelistErrorCode)
                .Must(CheckHtmlFreeTextBlacklist)
                .WithErrorCode(blacklistErrorCode);
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
