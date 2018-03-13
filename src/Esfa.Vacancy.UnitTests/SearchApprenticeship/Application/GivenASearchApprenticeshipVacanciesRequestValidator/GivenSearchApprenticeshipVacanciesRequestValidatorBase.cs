using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    public abstract class GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private Mock<ITrainingDetailService> _mockTrainingRepository;

        internal static List<TrainingDetail> ValidFrameworkCodes =>
            new List<TrainingDetail> { new TrainingDetail { TrainingCode = "1", FrameworkCode = 1, IsActive = true } };

        internal static List<TrainingDetail> ValidStandardCodes =>
            new List<TrainingDetail> { new TrainingDetail { TrainingCode = "1", IsActive = true } };
        internal SearchApprenticeshipVacanciesRequestValidator Validator { get; private set; }

        [SetUp]
        public void Setup()
        {
            _mockTrainingRepository = new Mock<ITrainingDetailService>();
            _mockTrainingRepository
                .Setup(r => r.GetAllStandardDetailsAsync())
                .ReturnsAsync(ValidStandardCodes);

            _mockTrainingRepository
                .Setup(r => r.GetAllFrameworkDetailsAsync())
                .ReturnsAsync(ValidFrameworkCodes);

            Validator = new SearchApprenticeshipVacanciesRequestValidator(_mockTrainingRepository.Object);
        }
    }
}
