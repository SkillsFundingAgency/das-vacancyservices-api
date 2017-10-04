using System.Linq;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.GivenSearchApprenticeshipParameters
{
    [TestFixture]
    public class AndStandardCodes
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperConfig.Configure();
        }

        [TestCase("1", 1, "One or more delimeted values are acceptable")]
        [TestCase("1,2", 2, "Each number should be delimeted by comma")]
        [TestCase("134, eaf, ef 3,234 2,  ,244, 2 ", 7, "Anything will be converted in to enumerable")]
        public void WhenValidInput_ThenReturnMappedRequestObject(string standardCodes, int count, string reason)
        {
            var parameters = new SearchApprenticeshipParameters() { StandardCodes = standardCodes };
            var result = Mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.StandardCodes.Count().Should().Be(count, reason);
        }

        [TestCase("", "Empty string will be ignored")]
        [TestCase(null, "Null will be ignored")]
        public void WhenNullOrEmpty_ThenReturnNull(string standardCodes, string reason)
        {
            var parameters = new SearchApprenticeshipParameters() { StandardCodes = standardCodes };
            var result = Mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.StandardCodes.Should().BeNull(reason);
        }

    }
}
