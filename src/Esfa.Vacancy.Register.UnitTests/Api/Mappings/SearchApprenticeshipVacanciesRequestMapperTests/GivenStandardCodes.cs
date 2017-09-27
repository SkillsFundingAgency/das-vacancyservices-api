using System.Linq;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.Api.Mappings.SearchApprenticeshipVacanciesRequestMapperTests
{
    [TestFixture]
    public class GivenStandardCodes
    {
        [Test]
        public void ShouldAppendStandardCodeIndicatorToEachCode()
        {
            var source = new SearchApprenticeshipParameters { StandardCodes = "123,456,789" };

            var expectedOutput = new[] { "STDSEC.123", "STDSEC.456", "STDSEC.789" };

            var target = SearchApprenticeshipVacanciesRequestMapper.Convert(source);

            target.StandardCodes.Should().Contain(expectedOutput);
        }

        [Test]
        public void ShouldIgnoreWhiteSpaceAndNonNumbers()
        {
            var source = new SearchApprenticeshipParameters { StandardCodes = "123, 456,789 , , wer" };

            var expectedOutput = new[] { "STDSEC.123", "STDSEC.456", "STDSEC.789" };

            var target = SearchApprenticeshipVacanciesRequestMapper.Convert(source);

            target.StandardCodes.Should().Contain(expectedOutput);
            target.StandardCodes.Count().ShouldBeEquivalentTo(3);
        }

        [Test]
        public void ShouldUseCommaDelimiter()
        {
            var source = new SearchApprenticeshipParameters { StandardCodes = "123, 456 789" };

            var expectedOutput = new[] { "STDSEC.123" };

            var target = SearchApprenticeshipVacanciesRequestMapper.Convert(source);

            target.StandardCodes.Should().Contain(expectedOutput);
            target.StandardCodes.Count().ShouldBeEquivalentTo(1);

        }
    }
}
