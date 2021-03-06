﻿using Esfa.Vacancy.Domain.Entities;
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
    public class WhenMappingLiveApprenticeshipWithCustomWageType
    {
        [TestCase(2, ApiTypes.WageUnit.Weekly)]
        [TestCase(3, ApiTypes.WageUnit.Monthly)]
        [TestCase(4, ApiTypes.WageUnit.Annually)]
        [TestCase(null, ApiTypes.WageUnit.Unspecified)]
        public void ShouldHaveWageSetForVacanciesWithCustomWageType(int? wageUnitId, ApiTypes.WageUnit expectedWageUnit)
        {
            const int weeklyWage = 2550;
            const int vacancyReference = 1234;
            const int liveVacancyStatusId = 2;

            var provideSettings = new Mock<IProvideSettings>();
            var sut = new Register.Api.Mappings.ApprenticeshipMapper(provideSettings.Object);

            var apprenticeshipVacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, vacancyReference)
                .With(v => v.ApprenticeshipTypeId, 1)
                .With(v => v.VacancyStatusId, liveVacancyStatusId)
                .With(v => v.WageType, (int)LegacyWageType.Custom)
                .With(v => v.WeeklyWage, weeklyWage)
                .Without(v => v.WageText)
                .With(v => v.WageUnitId, wageUnitId)
                .Create();

            var vacancy = sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(vacancyReference);
            vacancy.WageUnit.Should().Be(expectedWageUnit);
            vacancy.WageText.Should().Be("£2,550.00");
        }
    }
}
