using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenAnApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingLiveApprenticeshipWithAmbiguousWage
    {
        [TestCase(WageType.Unwaged, "Unwaged")]
        [TestCase(WageType.ToBeAgreedUponAppointment, "To be agreed upon appointment")]
        [TestCase(WageType.CompetitiveSalary, "Competitive salary")]
        public void ShouldHaveAppropriateWageDescription(WageType wageType, string expectedWageText)
        {
            const int vacancyReference = 1234;
            const int liveVacancyStatusId = 2;

            var provideSettings = new Mock<IProvideSettings>();
            var sut = new Register.Api.Mappings.ApprenticeshipMapper(provideSettings.Object);

            var apprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, vacancyReference)
                .With(v => v.VacancyStatusId, liveVacancyStatusId)
                .With(v => v.WageType, (int) wageType)
                .Without(v => v.WeeklyWage)
                .Without(v => v.WageUnitId)
                .Create();

            var vacancy = sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(vacancyReference);
            vacancy.WageUnit.Should().Be(WageUnit.Unspecified);
            vacancy.WageText.Should().Be(expectedWageText);
        }
    }
}
