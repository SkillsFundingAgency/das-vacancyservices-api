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
    [Parallelizable(ParallelScope.Fixtures)]
    public class WhenMappingLiveApprenticeshipWithApprenticeshipMinimumWageType
    {
        private const int VacancyReference = 1234;
        private const int LiveVacancyStatusId = 2;
        private const string UnknownwWageText = "Unknown";
        private readonly Mock<IProvideSettings> _provideSettings = new Mock<IProvideSettings>();
        private Register.Api.Mappings.ApprenticeshipMapper _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new Register.Api.Mappings.ApprenticeshipMapper(_provideSettings.Object);
        }

        [Test]
        public void ShouldHaveWageSetForVacancy()
        {
            const decimal minApprenticeshipWageRate = 4.05m;
            const decimal hoursPerWeek = 30;

            var apprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, VacancyReference)
                .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                .With(v => v.WageType, (int) WageType.ApprenticeshipMinimum)
                .With(v => v.MinimumWageRate, minApprenticeshipWageRate)
                .With(v => v.HoursPerWeek, hoursPerWeek)
                .Without(v => v.WageUnitId)
                .Create();

            var vacancy = _sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(VacancyReference);
            vacancy.WageUnit.Should().Be(WageUnit.Unspecified);
            vacancy.WageText.Should().Be("£121.50");
        }

        [Test]
        public void ShouldHaveUnknownWageForVacancyWithUndefinedHoursPerWeek()
        {
            const decimal minApprenticeshipWageRate = 4.05m;

            var apprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, VacancyReference)
                .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                .With(v => v.WageType, (int) WageType.ApprenticeshipMinimum)
                .With(v => v.MinimumWageRate, minApprenticeshipWageRate)
                .Without(v => v.HoursPerWeek)
                .Without(v => v.WageUnitId)
                .Create();

            var vacancy = _sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(VacancyReference);
            vacancy.WageUnit.Should().Be(WageUnit.Unspecified);
            vacancy.WageText.Should().Be(UnknownwWageText);
        }
    }
}
