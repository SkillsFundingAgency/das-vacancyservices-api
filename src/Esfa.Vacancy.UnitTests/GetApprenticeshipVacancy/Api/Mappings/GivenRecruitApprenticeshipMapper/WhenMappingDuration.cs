using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using SFA.DAS.Recruit.Vacancies.Client.Entities;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingDuration : RecruitApprenticeshipMapperBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
        }

        [TestCase(1, "Year", "1 Year", TestName = "And duration is one")]
        [TestCase(2, "Year", "2 Years", TestName = "And duration is more than one")]
        public void ThenMapDuration(int duration, string unit, string expectedOutput)
        {
            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.Wage = FixtureInstance.Build<Wage>()
                .With(w => w.Duration, duration)
                .With(w => w.DurationUnit, unit)
                .Create();

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            mappedVacancy.ExpectedDuration.Should().Be(expectedOutput);
        }

    }
}
