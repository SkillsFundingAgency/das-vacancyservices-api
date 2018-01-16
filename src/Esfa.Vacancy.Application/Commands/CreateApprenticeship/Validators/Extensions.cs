﻿using System;
using System.Text.RegularExpressions;
using Esfa.Vacancy.Domain.Validation;
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

        [Obsolete]
        public static IRuleBuilderOptions<string, string> MatchesAllowedFreeTextCharacters(
            this IRuleBuilder<string, string> rule, string errorCode, string propertyName)
        {
            return rule.Matches(RegexFreeTextWhitelist)
                .WithErrorCode(errorCode)
                .WithName(propertyName)
                .WithMessage(string.Format(ErrorMessages.CreateApprenticeship.Whitelist, propertyName));
        }

        public static IRuleBuilderOptions<CreateApprenticeshipRequest, string> MatchesAllowedFreeTextCharacters(
            this IRuleBuilder<CreateApprenticeshipRequest, string> rule, string errorCode, string propertyName)
        {
            return rule.Matches(RegexFreeTextWhitelist)
                .WithErrorCode(errorCode)
                .WithMessage(string.Format(ErrorMessages.CreateApprenticeship.Whitelist, propertyName));
        }

        [Obsolete]
        public static IRuleBuilderOptions<string, string> MatchesAllowedHtmlFreeTextCharacters(
            this IRuleBuilder<string, string> rule, string whitelistErrorCode, string blacklistErrorCode, string propertyName)
        {
            return rule.Matches(RegexHtmlFreeTextWhitelist)
                .WithErrorCode(whitelistErrorCode)
                .WithName(propertyName)
                .WithMessage(string.Format(ErrorMessages.CreateApprenticeship.Whitelist, propertyName))

                .Must(CheckHtmlFreeTextBlacklist)
                .WithErrorCode(blacklistErrorCode)
                .WithName(propertyName)
                .WithMessage(string.Format(ErrorMessages.CreateApprenticeship.HtmlBlacklist, propertyName));
        }

        public static IRuleBuilderOptions<CreateApprenticeshipRequest, string> MatchesAllowedHtmlFreeTextCharacters(
            this IRuleBuilder<CreateApprenticeshipRequest, string> rule, string whitelistErrorCode, string blacklistErrorCode, string propertyName)
        {
            return rule.Matches(RegexHtmlFreeTextWhitelist)
                .WithErrorCode(whitelistErrorCode)
                .WithMessage(string.Format(ErrorMessages.CreateApprenticeship.Whitelist, propertyName))

                .Must(CheckHtmlFreeTextBlacklist)
                .WithErrorCode(blacklistErrorCode)
                .WithMessage(string.Format(ErrorMessages.CreateApprenticeship.HtmlBlacklist, propertyName));
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
