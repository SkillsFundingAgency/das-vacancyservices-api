﻿using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.Api.Mappings.ApprenticeshipMapper
{
    [TestFixture]
    public class WhenGettingLiveVacancyWithCustomWageType
    {
        [TestCase(WageUnit.Weekly)]
        [TestCase(WageUnit.Monthly)]
        [TestCase(WageUnit.Annually)]
        [TestCase(WageUnit.NotApplicable)]
        public void ShouldHaveWageSetForVacanciesWithCustomWageType(WageUnit wageUnit)
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
                .With(v => v.WageType, (int) WageType.Custom)
                .With(v => v.WeeklyWage, weeklyWage)
                .Without(v => v.WageText)
                .With(v => v.WageUnitId, (int) wageUnit)
                .Create();

            var vacancy = sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(vacancyReference);
            vacancy.WageUnit.Should().Be(wageUnit);
            vacancy.WageText.Should().Be("£2,550.00");
        }
    }
}