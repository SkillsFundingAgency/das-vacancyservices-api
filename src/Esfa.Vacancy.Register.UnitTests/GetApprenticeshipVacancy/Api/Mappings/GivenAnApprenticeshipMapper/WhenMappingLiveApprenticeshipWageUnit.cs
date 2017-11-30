using System.ComponentModel;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenAnApprenticeshipMapper
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
            _sut = new Register.Api.Mappings.ApprenticeshipMapper(provideSettings.Object);
        }

        [Test]
        public void ShouldThrowErrorForUnknownWageUnitId()
        {
            var apprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                .With(v => v.WageType, (int)WageType.Custom)
                .With(v => v.WageUnitId, 99)
                .Create();
            
            Assert.Throws<InvalidEnumArgumentException>(() => _sut.MapToApprenticeshipVacancy(apprenticeshipVacancy));
        }

        [TestCase(2, WageUnit.Weekly, 1)]
        [TestCase(3, WageUnit.Monthly, 2)]
        [TestCase(4, WageUnit.Annually, 3)]
        [TestCase(null, WageUnit.Unspecified, 0)]
        public void ShouldMapWageUnitIdToWageUnitEnum(int? wageUnitId, WageUnit wageUnitType, int expectedWageUnitUnderlyingValue)
        {
            //Arrange
            var apprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                .With(v => v.WageType, (int) WageType.Custom)
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
