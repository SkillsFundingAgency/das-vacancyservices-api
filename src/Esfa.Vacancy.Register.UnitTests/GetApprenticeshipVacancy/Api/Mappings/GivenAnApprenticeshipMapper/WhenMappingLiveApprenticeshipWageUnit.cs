using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenAnApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingLiveApprenticeshipWageUnit
    {
        [TestCase(2, WageUnit.Weekly, 1)]
        [TestCase(3, WageUnit.Monthly, 2)]
        [TestCase(4, WageUnit.Annually, 3)]
        [TestCase(null, WageUnit.Unspecified, 0)]
        public void ShouldMapWageUnitIdToWageUnitEnum(int? wageUnitId, WageUnit wageUnitType, int expectedWageUnitUnderlyingValue)
        {
            //Arrange
            var provideSettings = new Mock<IProvideSettings>();
            var sut = new Register.Api.Mappings.ApprenticeshipMapper(provideSettings.Object);

            var apprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                .With(v => v.WageType, (int) WageType.Custom)
                .With(v => v.WageUnitId, wageUnitId)
                .Create();
            
            //Act
            var vacancy = sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);
            //Assert
            vacancy.WageUnit.ShouldBeEquivalentTo(wageUnitType);
            vacancy.WageUnit.ShouldBeEquivalentTo(expectedWageUnitUnderlyingValue);
        }
    }
}
