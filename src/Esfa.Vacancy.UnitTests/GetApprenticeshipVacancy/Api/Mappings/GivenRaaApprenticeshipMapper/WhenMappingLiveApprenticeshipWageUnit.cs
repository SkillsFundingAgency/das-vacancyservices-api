using System.ComponentModel;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ApiTypes = Esfa.Vacancy.Api.Types;
using ApprenticeshipVacancy = Esfa.Vacancy.Domain.Entities.ApprenticeshipVacancy;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRaaApprenticeshipMapper
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class WhenMappingLiveApprenticeshipWageUnit
    {
        private ApprenticeshipMapper _sut;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            var provideSettings = new Mock<IProvideSettings>();
            _sut = new ApprenticeshipMapper(provideSettings.Object);
        }

        [Test]
        public void ShouldThrowErrorForUnknownWageUnitId()
        {
            var apprenticeshipVacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.WageType, (int)LegacyWageType.Custom)
                .With(v => v.WageUnitId, 99)
                .Create();

            Assert.Throws<InvalidEnumArgumentException>(() => _sut.MapToApprenticeshipVacancy(apprenticeshipVacancy));
        }

        [TestCase(2, ApiTypes.WageUnit.Weekly, 1)]
        [TestCase(3, ApiTypes.WageUnit.Monthly, 2)]
        [TestCase(4, ApiTypes.WageUnit.Annually, 3)]
        [TestCase(null, ApiTypes.WageUnit.Unspecified, 0)]
        public void ShouldMapWageUnitIdToWageUnitEnum(int? wageUnitId, ApiTypes.WageUnit wageUnitType, int expectedWageUnitUnderlyingValue)
        {
            //Arrange
            var apprenticeshipVacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.WageType, (int)LegacyWageType.Custom)
                .With(v => v.ApprenticeshipTypeId, 1)
                .With(v => v.WageUnitId, wageUnitId)
                .Create();

            //Act
            var vacancy = _sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);
            //Assert
            vacancy.WageUnit.ShouldBeEquivalentTo(wageUnitType);
            vacancy.WageUnit.ShouldBeEquivalentTo(expectedWageUnitUnderlyingValue);
        }
    }
}
