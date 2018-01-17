using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingWithMatchesAllowedFreeTextCharacters
    {
        private const string ErrorCode = "ErrorCode";
        private const string PropertyName = "PropertyName";

        [Test]
        public void ThenCheckValidCharacters()
        {
            var validChars = GetValidCharacters();
            var request = new StubRequest{TestString = validChars};
            var sut = new TestMatchesAllowedHtmlFreeTextCharactersValidator();

            var result = sut.Validate(request);

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
                var request = new StubRequest{TestString = invalidChar.ToString()};
                var result = sut.Validate(request);

                result.IsValid.Should().BeFalse();
                result.Errors.Single().ErrorCode.Should().Be(ErrorCode);
                result.Errors.Single().ErrorMessage.Should().Be("'PropertyName' can't contain invalid characters");
            }
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
            text.Append(@"?$@#()""'!,+-=_:;.&€£*%/[]");

            //whitespace characters matched by \s regex
            text.Append(" \f\n\r\t\v");

            return text.ToString();
        }

        private string GetStringForCharacterCodeRange(int rangeStart, int rangeEnd)
        {
            return new string(Enumerable.Range(rangeStart, rangeEnd - rangeStart + 1).Select(i => (char)i).ToArray());
        }

        private class StubRequest
        {
            public string TestString { get; set; }
        }

        private class TestMatchesAllowedHtmlFreeTextCharactersValidator : AbstractValidator<StubRequest>
        {
            public TestMatchesAllowedHtmlFreeTextCharactersValidator()
            {
                RuleFor(request => request.TestString).MatchesAllowedFreeTextCharacters(ErrorCode, PropertyName);
            }
        }
    }
}
