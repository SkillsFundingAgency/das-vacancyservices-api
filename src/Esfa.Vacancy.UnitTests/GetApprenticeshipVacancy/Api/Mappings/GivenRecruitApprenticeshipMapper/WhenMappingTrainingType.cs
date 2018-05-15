using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ApiTypes = Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingTrainingType : RecruitApprenticeshipMapperBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
        }

        [TestCase("Standard", ApiTypes.TrainingType.Standard)]
        [TestCase("Framework", ApiTypes.TrainingType.Framework)]
        public void ThenMapTrainingType(string trainingType, ApiTypes.TrainingType expectedOuput)
        {
            TrainingDetailServiceMock
                .Setup(t => t.GetFrameworkDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(FixtureInstance.Create<Framework>());
            TrainingDetailServiceMock
                .Setup(t => t.GetStandardDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(FixtureInstance.Create<Standard>());

            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.ProgrammeType = trainingType;

            var mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;

            mappedVacancy.TrainingType.Should().Be(expectedOuput);
        }
    }
}
