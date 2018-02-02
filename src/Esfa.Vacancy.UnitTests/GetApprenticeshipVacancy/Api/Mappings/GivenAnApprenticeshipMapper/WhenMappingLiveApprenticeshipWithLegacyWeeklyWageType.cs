using ApiTypes = Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenAnApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingLiveApprenticeshipWithLegacyWeeklyWageType
    {
        [Test]
        public void LiveVacanciesWithLegacyWeeklyWageTypeShouldHaveWageSetFromWeeklyWage()
        {
            const int weeklyWage = 2550;
            const int vacancyReference = 1234;
            const int liveVacancyStatusId = 2;

            var provideSettings = new Mock<IProvideSettings>();
            var sut = new Register.Api.Mappings.ApprenticeshipMapper(provideSettings.Object);

            var apprenticeshipVacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, vacancyReference)
                .With(v => v.VacancyStatusId, liveVacancyStatusId)
                .With(v => v.WageType, (int)LegacyWageType.LegacyWeekly)
                .With(v => v.WeeklyWage, weeklyWage)
                .Without(v => v.WageText)
                .With(v => v.WageUnitId, 2)
                .Create();

            var vacancy = sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(vacancyReference);
            vacancy.WageUnit.Should().Be(ApiTypes.WageUnit.Weekly);
            vacancy.WageText.Should().Be("£2,550.00");
        }
    }
}
