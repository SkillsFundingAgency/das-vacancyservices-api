using System;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using SFA.DAS.Recruit.Vacancies.Client.Entities;

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
            var minWage = 8m;
            var maxWage = 10m;
            var weeklyHours = 40;
            var wageRange = FixtureInstance.Build<WageRange>()
                .With(w => w.NationalMinimumWage, minWage)
                .With(w => w.NationalMaximumWage, maxWage)
                .Create();

            MinimumWageServiceMock
                .Setup(s => s.GetWageRangeAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(wageRange);

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
            var minWage = 8m;
            var weeklyHours = 40;
            var wageRange = FixtureInstance.Build<WageRange>()
                .With(w => w.ApprenticeMinimumWage, minWage)
                .Create();

            MinimumWageServiceMock
                .Setup(s => s.GetWageRangeAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(wageRange);

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