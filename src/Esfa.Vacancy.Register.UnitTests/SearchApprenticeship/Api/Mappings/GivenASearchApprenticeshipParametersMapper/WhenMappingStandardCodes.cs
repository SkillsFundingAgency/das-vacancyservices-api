using System.Linq;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.Mappings.GivenASearchApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingStandardCodes
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            MapperConfiguration config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [TestCase("1", 1, "One or more delimeted values are acceptable")]
        [TestCase("1,2", 2, "Each number should be delimeted by comma")]
        [TestCase("134, eaf, ef 3,234 2,  ,244, 2 ", 7, "Anything will be converted in to enumerable")]
        public void WhenValidInput_ThenReturnMappedRequestObject(string standardCodes, int count, string reason)
        {
            var parameters = new SearchApprenticeshipParameters() { StandardLarsCodes = standardCodes };
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.StandardLarsCodes.Count().Should().Be(count, reason);
        }

        [TestCase("", "Empty string will be ignored")]
        [TestCase(null, "Null will be ignored")]
        public void WhenNullOrEmpty_ThenReturnEmptyList(string standardCodes, string reason)
        {
            var parameters = new SearchApprenticeshipParameters() { StandardLarsCodes = standardCodes };
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.StandardLarsCodes.Should().BeEmpty(reason);
        }
    }
}
