using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using SFA.DAS.Recruit.Vacancies.Client.Entities;
using ApiTypes = Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingWageUnit : RecruitApprenticeshipMapperBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
        }

        [TestCase(RecruitApprenticeshipMapper.FixedWageType, ApiTypes.WageUnit.Annually, TestName = "And wage type is custom then should be Yearly.")]
        [TestCase("NationalMinimumWageForApprentices", ApiTypes.WageUnit.Weekly, TestName = "And wage type is custom then should be Yearly.")]
        [TestCase("NationalMinimumWage", ApiTypes.WageUnit.Weekly, TestName = "And wage type is custom then should be Yearly.")]
        [TestCase("Unspecified", ApiTypes.WageUnit.Unspecified, TestName = "And wage type is custom then should be Yearly.")]
        public void ThenMapWageUnit(string wageType, ApiTypes.WageUnit wageUnit)
        {
            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.Wage = FixtureInstance.Build<Wage>().With(w => w.WageType, wageType).Create();

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            mappedVacancy.WageUnit.Should().Be(wageUnit);
        }
    }
}