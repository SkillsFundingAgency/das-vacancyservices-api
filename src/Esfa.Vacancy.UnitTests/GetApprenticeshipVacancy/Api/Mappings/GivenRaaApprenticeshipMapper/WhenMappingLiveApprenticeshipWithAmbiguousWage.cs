using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ApiTypes = Esfa.Vacancy.Api.Types;
using ApprenticeshipVacancy = Esfa.Vacancy.Domain.Entities.ApprenticeshipVacancy;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRaaApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingLiveApprenticeshipWithAmbiguousWage
    {
        [TestCase(LegacyWageType.Unwaged, "Unwaged")]
        [TestCase(LegacyWageType.ToBeAgreedUponAppointment, "To be agreed upon appointment")]
        [TestCase(LegacyWageType.CompetitiveSalary, "Competitive salary")]
        public void ShouldHaveAppropriateWageDescription(LegacyWageType legacyWageType, string expectedWageText)
        {
            const int vacancyReference = 1234;
            const int liveVacancyStatusId = 2;

            var provideSettings = new Mock<IProvideSettings>();
            var sut = new Register.Api.Mappings.ApprenticeshipMapper(provideSettings.Object);

            var apprenticeshipVacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, vacancyReference)
                .With(v => v.VacancyStatusId, liveVacancyStatusId)
                .With(v => v.WageType, (int)legacyWageType)
                .Without(v => v.WeeklyWage)
                .Without(v => v.WageUnitId)
                .Create();

            var vacancy = sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(vacancyReference);
            vacancy.WageUnit.Should().Be(ApiTypes.WageUnit.Unspecified);
            vacancy.WageText.Should().Be(expectedWageText);
        }
    }
}
