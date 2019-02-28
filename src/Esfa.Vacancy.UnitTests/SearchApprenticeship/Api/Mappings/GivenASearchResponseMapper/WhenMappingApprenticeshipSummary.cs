using AutoMapper;
using Esfa.Vacancy.Register.Api;
using FluentAssertions;
using NUnit.Framework;
using ApiTypes = Esfa.Vacancy.Api.Types;
using ApprenticeshipSummary = Esfa.Vacancy.Domain.Entities.ApprenticeshipSummary;
using GeoPoint = Esfa.Vacancy.Domain.Entities.GeoPoint;


namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Api.Mappings.GivenASearchResponseMapper
{
    [TestFixture]
    public class WhenMappingApprenticeshipSummary
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [TestCase(1, null, ApiTypes.TrainingType.Standard, TestName = "And has Standard ID then TrainingType set to Standard")]
        [TestCase(null, "10", ApiTypes.TrainingType.Framework, TestName = "And has Framework Code then TrainingType set to Framework")]
        public void AndMappingTrainingType(int? standardId, string frameworkCode, ApiTypes.TrainingType expectedTrainingType)
        {
            var expectedTrainingCode = standardId.HasValue ? standardId.ToString() : frameworkCode;
            var domainType = new ApprenticeshipSummary()
            {
                FrameworkLarsCode = frameworkCode,
                StandardLarsCode = standardId
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.TrainingType.Should().Be(expectedTrainingType);
            result.TrainingCode.Should().Be(expectedTrainingCode);
        }

        [Test]
        public void ThenGeoCoordinatesMapCorrectly()
        {
            var domainType = new ApprenticeshipSummary
            {
                Location = new GeoPoint() { Lat = 51.3288148990, Lon = 0.4452948632 }
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.Location.Latitude.Should().Be(51.3288148990m, "Then map latitude to Location");
            result.Location.Longitude.Should().Be(0.4452948632m, "Then map longitude to Location");
        }

        [Test]
        public void ThenShortDescriptionMapsCorrectly()
        {
            var description = "desc";
            var domainType = new ApprenticeshipSummary
            {
                Description = description
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.ShortDescription.Should().Be(description, "Then map Description to ShortDescription");
        }

        [Test]
        public void ThenTrainingProviderNameMapsCorrectly()
        {
            var providerName = "desc";
            var domainType = new ApprenticeshipSummary
            {
                ProviderName = providerName
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.TrainingProviderName.Should().Be(providerName, "Then map ProviderName to TrainingProviderName");
        }

        [TestCase("National", true, TestName = "And Location Type is National Then map set IsNationWide to true")]
        [TestCase("NonNational", false, TestName = "And Location Type is NonNational Then map set IsNationWide to false")]
        public void AndMappingIsNationWide(string value, bool expectedResult)
        {
            var domainType = new ApprenticeshipSummary
            {
                VacancyLocationType = value
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.IsNationwide.Should().Be(expectedResult);
        }

        [Test]
        public void ThenDistanceInMilesMapsCorrectly()
        {
            var expectedDistance = 3245.3245;
            var domainType = new ApprenticeshipSummary
            {
                DistanceInMiles = expectedDistance
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.DistanceInMiles.Should().Be(expectedDistance);
        }

        [Test]
        public void ThenProviderUkprnMapsCorrectly()
        {
            var ukprn = 88888888;
            var domainType = new ApprenticeshipSummary
            {
                ProviderUkprn = ukprn
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.Ukprn.Should().Be(ukprn);
        }
    }
}
