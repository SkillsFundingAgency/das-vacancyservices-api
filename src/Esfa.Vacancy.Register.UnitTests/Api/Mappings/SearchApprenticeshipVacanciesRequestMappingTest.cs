using System.Linq;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.App_Start;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Esfa.Vacancy.Register.UnitTests.Api.Mappings
{
    [TestFixture]
    public class SearchApprenticeshipVacanciesRequestMappingTest
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperConfig.Configure();
        }

        [TestCase("1", 1, "One or more delimeted values are acceptable")]
        [TestCase("1,2", 2, "Each number should be delimeted by comma")]
        [TestCase("134, eaf, ef 3,234 2,  ,244, 2 ", 7, "Anything will be converted in to enumerable")]
        public void ShouldMapCorrectly(string standardCodes, int count, string reason)
        {
            var parameters = new SearchApprenticeshipParameters() { StandardCodes = standardCodes };
            var result = Mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.StandardCodes.Count().Should().Be(count, reason);
        }

        [TestCase("", "Empty string will be ignored")]
        [TestCase(null, "Null will be ignored")]
        public void NullAndEmptyWillNotMap(string standardCodes, string reason)
        {
            var parameters = new SearchApprenticeshipParameters() { StandardCodes = standardCodes };
            var result = Mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.StandardCodes.Should().BeNull(reason);
        }

    }
}
