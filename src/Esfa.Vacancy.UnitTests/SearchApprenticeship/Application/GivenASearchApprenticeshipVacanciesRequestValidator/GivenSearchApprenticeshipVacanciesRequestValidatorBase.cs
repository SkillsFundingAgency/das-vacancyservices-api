using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    public abstract class GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private Mock<ITrainingDetailService> _mockTrainingRepository;

        private static List<TrainingDetail> _validFrameworkCodes =>
            new List<TrainingDetail> { new TrainingDetail { TrainingCode = "1", FrameworkCode = 1, IsActive = true } };

        internal static List<TrainingDetail> _validStandardCodes =>
            new List<TrainingDetail> { new TrainingDetail { TrainingCode = "1", IsActive = true } };

        internal static List<string> ValidFrameworkCodes =>
            _validFrameworkCodes.Select(fwk => fwk.TrainingCode).ToList();

        internal static List<string> ValidStandardCodes =>
            _validStandardCodes.Select(std => std.TrainingCode).ToList();

        internal SearchApprenticeshipVacanciesRequestValidator Validator { get; private set; }

        [SetUp]
        public void Setup()
        {
            _mockTrainingRepository = new Mock<ITrainingDetailService>();
            _mockTrainingRepository
                .Setup(r => r.GetAllStandardDetailsAsync())
                .ReturnsAsync(_validStandardCodes);

            _mockTrainingRepository
                .Setup(r => r.GetAllFrameworkDetailsAsync())
                .ReturnsAsync(_validFrameworkCodes);

            Validator = new SearchApprenticeshipVacanciesRequestValidator(_mockTrainingRepository.Object);
        }
    }
}
