using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingWithMatchesAllowedHtmlFreeTextCharacters
    {

        private const string WhitelistErrorCode = "WhitelistErrorCode";
        private const string BlacklistErrorCode = "BlackistErrorCode";
        private const string PropertyName = "PropertyName";

        [Test]
        public void ThenCheckValidCharacters()
        {
            var validChars = GetValidCharacters();

            var sut = new TestMatchesAllowedHtmlFreeTextCharactersValidator();

            sut.Validate(validChars);

            var result = sut.Validate(validChars);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ThenCheckInvalidCharacters()
        {
            var validChars = GetValidCharacters().ToCharArray().Select(c => (int)c).ToArray();

            //65450 is just an arbitary number that is greater than 65447. Any characters greater than 65447 will fail.
            var allChars = Enumerable.Range(0, 65450).ToArray();

            var invalidChars = allChars.Except(validChars).Select(i => (char)i).ToArray();

            var sut = new TestMatchesAllowedHtmlFreeTextCharactersValidator();

            foreach (var invalidChar in invalidChars)
            {
                var result = sut.Validate(invalidChar.ToString());

                result.IsValid.Should().BeFalse();
                result.Errors.Single().ErrorCode.Should().Be(WhitelistErrorCode);
                result.Errors.Single().ErrorMessage.Should().Be("'PropertyName' can't contain invalid characters");
            }
            
        }

        [TestCase("< i n p u t >")]
        [TestCase("< o b j e c t >")]
        [TestCase("< s c r i p t >")]
        public void ThenCheckBlacklistHtmlElements(string text)
        {
            var sut = new TestMatchesAllowedHtmlFreeTextCharactersValidator();

            var result = sut.Validate(text);

            result.IsValid.Should().BeFalse();
            result.Errors.First().ErrorCode.Should().Be(BlacklistErrorCode);
        }

        private string GetValidCharacters()
        {
            var text = new StringBuilder();

            //a-z char codes 97-122
            text.Append(GetStringForCharacterCodeRange(97, 122));

            //A-Z char codes 65-90
            text.Append(GetStringForCharacterCodeRange(65, 90));

            //0-9 char codes 48-57
            text.Append(GetStringForCharacterCodeRange(48, 57));

            //\u0080-\uFFA7 char codes 128-65447
            text.Append(GetStringForCharacterCodeRange(128, 65447));

            //specific characters
            text.Append(@"?$@#()""'!,+-=_:;.&€£*%/<>[]");

            //whitespace characters matched by \s regex
            text.Append(" \f\n\r\t\v");

            return text.ToString();
        }

        private string GetStringForCharacterCodeRange(int rangeStart, int rangeEnd)
        {
            return new string(Enumerable.Range(rangeStart, rangeEnd - rangeStart + 1).Select(i => (char)i).ToArray());
        }

        private class TestMatchesAllowedHtmlFreeTextCharactersValidator : AbstractValidator<string>
        {
            public TestMatchesAllowedHtmlFreeTextCharactersValidator()
            {
                RuleFor(s => s).MatchesAllowedHtmlFreeTextCharacters(WhitelistErrorCode, BlacklistErrorCode, PropertyName);
            }
        }
    }
}
