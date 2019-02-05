using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture]
    public class AndNationwideOnly
    {
        private VacancySearchParametersMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mapper = fixture.Create<VacancySearchParametersMapper>();
        }

        [Test]
        public void AndIsTrue_ThenMapsToNationwide()
        {
            var result = _mapper.Convert(new SearchApprenticeshipVacanciesRequest
                { NationwideOnly = true });

            result.LocationType.Should().Be(VacancySearchParametersMapper.NationwideLocationType);
        }

        [Test]
        public void AndIsTrue_ThenGeoCodeFieldsAreSetToNull()
        {
            var result = _mapper.Convert(new SearchApprenticeshipVacanciesRequest
                { NationwideOnly = true, Latitude = 1, Longitude = 1, DistanceInMiles = 1});

            result.Latitude.Should().BeNull();
            result.Longitude.Should().BeNull();
            result.DistanceInMiles.Should().BeNull();
        }

        [Test]
        public void AndIsFalse_ThenDoesNotMap()
        {
            var result = _mapper.Convert(new SearchApprenticeshipVacanciesRequest
                { NationwideOnly = false });

            result.LocationType.Should().BeNullOrEmpty();
        }
    }
}