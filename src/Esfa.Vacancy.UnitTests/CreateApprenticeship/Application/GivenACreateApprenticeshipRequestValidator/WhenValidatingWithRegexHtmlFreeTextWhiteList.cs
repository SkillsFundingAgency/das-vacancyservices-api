using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingWithRegexHtmlFreeTextWhiteList
    {
        [Test]
        public void ThenCheckValidCharacters()
        {
            var validChars = GetValidCharacters();

            var match = Regex.Match(validChars, CreateApprenticeshipRequestValidator.RegexHtmlFreeTextWhitelist);

            match.Success.Should().Be(true);
        }

        [Test]
        public void ThenCheckInvalidCharacters()
        {
            var validChars = GetValidCharacters().ToCharArray().Select(c => (int)c).ToArray();

            //65450 is just an arbitary number that is greater than 65447. Any characters greater than 65447 will fail.
            var allChars = Enumerable.Range(0, 65450).ToArray();

            var invalidChars = allChars.Except(validChars).Select(i => (char)i).ToArray();

            foreach (var invalidChar in invalidChars)
            {                
                var match = Regex.Match(invalidChar.ToString(), CreateApprenticeshipRequestValidator.RegexHtmlFreeTextWhitelist);

                match.Success.Should().Be(false);
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
            text.Append(@"?$@#()""'!,+-=_:;.&€£*%/<>[]");

            //whitespace characters matched by \s regex
            text.Append(" \f\n\r\t\v");

            return text.ToString();
        }

        private string GetStringForCharacterCodeRange(int rangeStart, int rangeEnd)
        {
            return new string(Enumerable.Range(rangeStart, rangeEnd - rangeStart + 1).Select(i => (char)i).ToArray());
        }
    }
}
