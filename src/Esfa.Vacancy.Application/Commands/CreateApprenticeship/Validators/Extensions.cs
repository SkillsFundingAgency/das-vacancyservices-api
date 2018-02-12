using System.Text.RegularExpressions;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public static class Extensions
    {
        private const string RegexFreeTextWhitelist = @"^[a-zA-Z0-9\u0080-\uFFA7?$@#()""'!,+\-=_:;.&€£*%\s\/\[\]]*$";
        private const string RegexHtmlFreeTextWhitelist = @"^[a-zA-Z0-9\u0080-\uFFA7?$@#()""'!,+\-=_:;.&€£*%\s\/<>\[\]]+$";
        private const string RegexScriptsBlacklist = @"<\s*s\s*c\s*r\s*i\s*p\s*t\s*[^>]*\s*[^>]*\s*[^>]*>";
        private const string RegexInputsBlacklist = @"<\s*i\s*n\s*p\s*u\s*t\s*[^>]*\s*[^>]*\s*[^>]*>";
        private const string RegexObjectsBlacklist = @"<\s*o\s*b\s*j\s*e\s*c\s*t\s*[^>]*\s*[^>]*\s*[^>]*>";
        // See http://stackoverflow.com/questions/164979/uk-postcode-regex-comprehensive
        private const string RegexPostcode = @"^(([gG][iI][rR] {0,}0[aA]{2})|((([a-pr-uwyzA-PR-UWYZ][a-hk-yA-HK-Y]?[0-9][0-9]?)|(([a-pr-uwyzA-PR-UWYZ][0-9][a-hjkstuwA-HJKSTUW])|([a-pr-uwyzA-PR-UWYZ][a-hk-yA-HK-Y][0-9][abehmnprv-yABEHMNPRV-Y]))) {0,}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2}))$";
        private const string RegexEmailAddress = @"^[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+@[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+\.([a-zA-Z0-9\u0080-\uFFA7]{2,10})$";
        private const string RegexPhoneNumber = @"^[0-9+\s-()]{8,16}$";
        private const string RegexNameWhiteList = @"^[a-zA-Z()',+\-\s]+$";

        public static IRuleBuilderOptions<T, string> MustBeAValidName<T>(
            this IRuleBuilder<T, string> rule)
        {
            return rule.Matches(RegexNameWhiteList)
                .WithMessage(ErrorMessages.CreateApprenticeship.InvalidPropertyValue);
        }

        public static IRuleBuilderOptions<T, string> MustBeAValidPhoneNumber<T>(
            this IRuleBuilder<T, string> rule)
        {
            return rule.Matches(RegexPhoneNumber)
                .WithMessage(ErrorMessages.CreateApprenticeship.InvalidPropertyValue);
        }

        public static IRuleBuilderOptions<T, string> MustBeAValidEmailAddress<T>(
            this IRuleBuilder<T, string> rule)
        {
            return rule.Matches(RegexEmailAddress)
                .WithMessage(ErrorMessages.CreateApprenticeship.InvalidPropertyValue);
        }

        public static IRuleBuilderOptions<T, string> MustBeAValidPostcode<T>(
            this IRuleBuilder<T, string> rule)
        {
            return rule.Matches(RegexPostcode)
                .WithMessage(ErrorMessages.CreateApprenticeship.InvalidPropertyValue);
        }

        public static IRuleBuilderOptions<T, string> MatchesAllowedFreeTextCharacters<T>(
            this IRuleBuilder<T, string> rule)
        {
            return rule.Matches(RegexFreeTextWhitelist)
                .WithMessage(ErrorMessages.CreateApprenticeship.WhitelistFailed);
        }

        public static IRuleBuilderOptions<T, string> MatchesAllowedHtmlFreeTextCharacters<T>(
            this IRuleBuilder<T, string> rule, string errorCode)
        {
            return rule.Matches(RegexHtmlFreeTextWhitelist)
                .WithErrorCode(errorCode)
                .WithMessage(ErrorMessages.CreateApprenticeship.WhitelistFailed)

                .Must(CheckHtmlFreeTextBlacklist)
                .WithErrorCode(errorCode)
                .WithMessage(ErrorMessages.CreateApprenticeship.HtmlBlacklistFailed);
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
