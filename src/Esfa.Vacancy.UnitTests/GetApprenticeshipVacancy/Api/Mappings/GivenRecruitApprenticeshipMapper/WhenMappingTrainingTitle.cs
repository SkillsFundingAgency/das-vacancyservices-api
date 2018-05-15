using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingTrainingTitle : RecruitApprenticeshipMapperBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
        }

        [Test]
        public void AndTrainingTypeIsStandard()
        {
            var standard = FixtureInstance.Create<Standard>();
            TrainingDetailServiceMock
                .Setup(t => t.GetStandardDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(standard);

            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.ProgrammeType = "Standard";

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            mappedVacancy.TrainingTitle.Should().Be(standard.Title);

            TrainingDetailServiceMock.Verify(t => t.GetFrameworkDetailsAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AndTrainingTypeIsFramework()
        {
            var framework = FixtureInstance.Create<Framework>();
            TrainingDetailServiceMock
                .Setup(t => t.GetFrameworkDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(framework);

            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.ProgrammeType = "Framework";

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            mappedVacancy.TrainingTitle.Should().Be(framework.Title);

            TrainingDetailServiceMock.Verify(t => t.GetStandardDetailsAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
