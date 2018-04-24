using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenAnApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingLiveApprenticeshipEducationLevel
    {
        private ApprenticeshipMapper _sut;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            var provideSettings = new Mock<IProvideSettings>();
            _sut = new ApprenticeshipMapper(provideSettings.Object);
        }

        [TestCase(1, "Intermediate", TestName = "And ApprenticeshipTypeId is 1 Then EducationLevel is Intermediate")]
        [TestCase(2, "Advanced", TestName = "And ApprenticeshipTypeId is 2 Then EducationLevel is Advanced")]
        [TestCase(3, "Higher", TestName = "And ApprenticeshipTypeId is 3 Then EducationLevel is Higher")]
        [TestCase(5, "Foundation", TestName = "And ApprenticeshipTypeId is 5 Then EducationLevel is Foundation")]
        [TestCase(6, "Degree", TestName = "And ApprenticeshipTypeId is 6 Then EducationLevel is Degree")]
        [TestCase(7, "Masters", TestName = "And ApprenticeshipTypeId is 7 Then EducationLevel is Masters")]
        public void MapApprenticeshipTypeValid(int apprenticeshipTypeId, string educationLevel)
        {
            var vacancy = new ApprenticeshipVacancy
            {
                ApprenticeshipTypeId = apprenticeshipTypeId,
                Location = new Address()
            };

            var result = _sut.MapToApprenticeshipVacancy(vacancy);

            result.EducationLevel.Should().Be(educationLevel);
        }
    }
}
