using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingTrainingCode : RecruitApprenticeshipMapperBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
        }

        [TestCase("Standard", "123", "123")]
        [TestCase("Framework", "111-2-33", "111")]
        public void ThenMapTrainingCode(string trainingType, string trainingCode, string expectedOuput)
        {
            TrainingDetailServiceMock
                .Setup(t => t.GetFrameworkDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(FixtureInstance.Create<Framework>());
            TrainingDetailServiceMock
                .Setup(t => t.GetStandardDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(FixtureInstance.Create<Standard>());

            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.ProgrammeType = trainingType;
            LiveVacancy.ProgrammeId = trainingCode;

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            mappedVacancy.TrainingCode.Should().Be(expectedOuput);
        }
    }
}
