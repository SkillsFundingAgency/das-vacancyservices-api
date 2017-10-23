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
    public class WhenMappingLiveApprenticeshipWithCustomRangeWageType
    {
        [TestCase(2, WageUnit.Weekly, 14000, 16000, "£14,000.00 - £16,000.00")]
        [TestCase(2, WageUnit.Weekly, null, 16000, "Unknown - £16,000.00")]
        [TestCase(2, WageUnit.Weekly, 14000, null, "£14,000.00 - Unknown")]
        [TestCase(3, WageUnit.Monthly, 14000, 16000, "£14,000.00 - £16,000.00")]
        [TestCase(3, WageUnit.Monthly, null, 16000, "Unknown - £16,000.00")]
        [TestCase(3, WageUnit.Monthly, 14000, null, "£14,000.00 - Unknown")]
        [TestCase(4, WageUnit.Annually, 14000, 16000, "£14,000.00 - £16,000.00")]
        [TestCase(4, WageUnit.Annually, null, 16000, "Unknown - £16,000.00")]
        [TestCase(4, WageUnit.Annually, 14000, null, "£14,000.00 - Unknown")]
        public void ShouldHaveWageSetForVacanciesWithCustomRangeWageType(int wageUnitId, WageUnit expectedWageUnit, decimal? lowerBound, decimal? upperBound, string expectedWageText)
        {
            const int weeklyWage = 2550;
            const int vacancyReference = 1234;
            const int liveVacancyStatusId = 2;

            var provideSettings = new Mock<IProvideSettings>();
            var sut = new Register.Api.Mappings.ApprenticeshipMapper(provideSettings.Object);

            var apprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, vacancyReference)
                .With(v => v.VacancyStatusId, liveVacancyStatusId)
                .With(v => v.VacancyTypeId, (int) VacancyType.Apprenticeship)
                .With(v => v.WageLowerBound, lowerBound)
                .With(v => v.WageUpperBound, upperBound)
                .With(v => v.WageType, (int) WageType.CustomRange)
                .With(v => v.WeeklyWage, weeklyWage)
                .Without(v => v.WageText)
                .With(v => v.WageUnitId, wageUnitId)
                .Create();

            var vacancy = sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(vacancyReference);
            vacancy.WageUnit.Should().Be(expectedWageUnit);
            vacancy.WageText.Should().Be(expectedWageText);
        }
    }
}
