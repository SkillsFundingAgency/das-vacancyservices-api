using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using DomainTypes = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenAnApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingIsNationwide
    {
        private ApprenticeshipMapper _sut;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            var provideSettings = new Mock<IProvideSettings>();
            _sut = new ApprenticeshipMapper(provideSettings.Object);
        }

        [TestCase(1, false, TestName = "And LocationTypeId is 1 Then set IsNationwide to false")]
        [TestCase(2, false, TestName = "And LocationTypeId is 2 Then set IsNationwide to false")]
        [TestCase(3, true, TestName = "And LocationTypeId is 3 Then set IsNationwide to true")]
        public void ShouldMapUsingLocationTypeId(int vacancyLocationTypeid, bool expectedResult)
        {
            var vacancy = new DomainTypes.ApprenticeshipVacancy()
            {
                VacancyLocationTypeId = vacancyLocationTypeid,
                Location = new DomainTypes.Address()
            };

            var result = _sut.MapToApprenticeshipVacancy(vacancy);

            result.IsNationwide.Should().Be(expectedResult);
        }
    }
}
