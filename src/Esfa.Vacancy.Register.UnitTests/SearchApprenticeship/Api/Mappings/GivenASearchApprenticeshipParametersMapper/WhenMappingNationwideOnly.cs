using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Api;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.Mappings.GivenASearchApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingNationwideOnly
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [Test]
        public void AndValueIsTrue_ThenMapsToTrue()
        {
            var parameters = new SearchApprenticeshipParameters { NationwideOnly = true };
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.NationwideOnly.Should().Be(true);
        }

        [Test]
        public void AndValueIsNotSet_ThenMapsToFalse()
        {
            var parameters = new SearchApprenticeshipParameters();
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.NationwideOnly.Should().Be(false);
        }
    }
}