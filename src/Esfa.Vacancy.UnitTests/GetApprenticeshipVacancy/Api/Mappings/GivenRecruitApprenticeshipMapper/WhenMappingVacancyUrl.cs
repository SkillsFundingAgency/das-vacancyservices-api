using Esfa.Vacancy.Domain.Constants;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingVacancyUrl : RecruitApprenticeshipMapperBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
        }

        [Test]
        public void ThanMapVacancyUrl()
        {
            var baseUrl = FixtureInstance.Create<string>();
            ProvideSettingsMock
                .Setup(p => p.GetSetting(ApplicationSettingKeys.LiveApprenticeshipVacancyBaseUrlKey))
                .Returns(baseUrl);
            var sut = GetRecruitApprecticeshipMapper();

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            mappedVacancy.VacancyUrl.Should().Be($"{baseUrl}/{LiveVacancy.VacancyReference}"); 
        }
    }
}
