using System;
using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using SFA.DAS.Recruit.Vacancies.Client.Entities;
using SFA.DAS.VacancyServices.Wage;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingWageText : RecruitApprenticeshipMapperBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
        }

        [Test]
        public void AndWageTypeIsFixedWage()
        {
            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.Wage = FixtureInstance.Build<Wage>()
                .With(w => w.WageType, RecruitApprenticeshipMapper.FixedWageType)
                .Create();

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            mappedVacancy.WageText.Should()
                .Be(GetFormattedCurrencyString(LiveVacancy.Wage.FixedWageYearlyAmount.GetValueOrDefault()));
        }

        [Test]
        public void AndWageTypeIsUnspecified()
        {
            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.Wage = FixtureInstance.Build<Wage>()
                .With(w => w.WageType, RecruitApprenticeshipMapper.UnspecifiedWageType)
                .Create();

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            mappedVacancy.WageText.Should().Be(RecruitApprenticeshipMapper.UnknownText);
        }

        [Test]
        public void AndWageTypeIsNationalMinimumWage()
        {
            var minWage = 4.2m;
            var maxWage = 7.83m;
            var weeklyHours = 40;

            MinimumWageServiceMock
                .Setup(s => s.GetWageRange(It.IsAny<DateTime>()))
                .Returns<DateTime>(NationalMinimumWageService.GetHourlyRates);

            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.Wage = FixtureInstance.Build<Wage>()
                .With(w => w.WageType, RecruitApprenticeshipMapper.NationalMinimumWageWageType)
                .With(w => w.WeeklyHours, weeklyHours)
                .Create();

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            var expectedOutput = GetFormattedCurrencyString(minWage * weeklyHours) + " - " + GetFormattedCurrencyString(maxWage * weeklyHours);

            mappedVacancy.WageText.Should().Be(expectedOutput);
        }

        [Test]
        public void AndWageTypeIsNationalMinimumWageForApprentices()
        {
            var minWage = 3.7m;
            var weeklyHours = 40;

            MinimumWageServiceMock
                .Setup(s => s.GetWageRange(It.IsAny<DateTime>()))
                .Returns<DateTime>(NationalMinimumWageService.GetHourlyRates);

            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.Wage = FixtureInstance.Build<Wage>()
                .With(w => w.WageType, RecruitApprenticeshipMapper.NationalMinimumWageForApprenticesWageType)
                .With(w => w.WeeklyHours, weeklyHours)
                .Create();

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            var expectedOutput = GetFormattedCurrencyString(minWage * weeklyHours);

            mappedVacancy.WageText.Should().Be(expectedOutput);
        }
    }
}