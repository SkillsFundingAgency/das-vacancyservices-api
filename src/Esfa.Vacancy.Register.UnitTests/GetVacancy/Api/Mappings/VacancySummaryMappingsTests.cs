using AutoMapper;
using Esfa.Vacancy.Register.Api;
using FluentAssertions;
using NUnit.Framework;
using ApiTypes = Esfa.Vacancy.Api.Types;
using DomainTypes = Esfa.Vacancy.Register.Domain.Entities;


namespace Esfa.Vacancy.Register.UnitTests.GetVacancy.Api.Mappings
{
    public class VacancySummaryMappingsTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            AutoMapperConfig.Configure();
        }

        [TestCase("STDSEC.10", ApiTypes.TrainingType.Standard, "10")]
        [TestCase("FW.10", ApiTypes.TrainingType.Framework, "10")]
        public void LoadCorrectTraingingDetails(string subCategoryCode, ApiTypes.TrainingType expectedTrainingType, string code)
        {
            var domainType = new DomainTypes.ApprenticeshipSummary()
            {
                SubCategoryCode = subCategoryCode
            };

            var result = Mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.TrainingType.Should().Be(expectedTrainingType);
            result.TrainingCode.Should().Be(code);
        }

        [Test]
        public void LocationMappingTest()
        {
            var domainType = new DomainTypes.ApprenticeshipSummary
            {
                Location = new DomainTypes.GeoPoint() { Lat = 12.1213, Lon = 34.2343424 }
            };

            var result = Mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.Location.Latitude.Should().Be(12.1213);
            result.Location.Longitude.Should().Be(34.2343424);

        }
    }
}
