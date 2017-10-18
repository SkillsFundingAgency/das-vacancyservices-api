using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.Api.Mappings.ApprenticeshipMapper
{
    [TestFixture]
    public class WhenGettingLiveVacancyWage
    {
        [TestCase(1, WageUnit.NotApplicable)]
        [TestCase(2, WageUnit.Weekly)]
        [TestCase(3, WageUnit.Monthly)]
        [TestCase(4, WageUnit.Annually)]
        [TestCase(null, null)]
        public void ShouldMapWageUnitEnum(int? wageUnitId, WageUnit? wageUnitType)
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
        }
    }
}
