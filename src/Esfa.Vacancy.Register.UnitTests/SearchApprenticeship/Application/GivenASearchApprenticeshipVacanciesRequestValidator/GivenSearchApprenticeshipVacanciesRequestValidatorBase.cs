using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    public abstract class GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private Mock<IFrameworkCodeRepository> _mockFrameworkCodeRepository;
        private Mock<IStandardRepository> _mockStandardRepository;

        internal static List<string> ValidFrameworkCodes => new List<string> { "1" };
        internal static List<string> ValidStandardCodes => new List<string> { "1" };
        internal SearchApprenticeshipVacanciesRequestValidator Validator { get; private set; }

        [SetUp]
        public void Setup()
        {
            _mockStandardRepository = new Mock<IStandardRepository>();
            _mockStandardRepository
                .Setup(r => r.GetStandardIdsAsync())
                .ReturnsAsync(ValidStandardCodes.Select(int.Parse));

            _mockFrameworkCodeRepository = new Mock<IFrameworkCodeRepository>();
            _mockFrameworkCodeRepository
                .Setup(r => r.GetAsync())
                .ReturnsAsync(ValidFrameworkCodes);

            Validator = new SearchApprenticeshipVacanciesRequestValidator(
                _mockFrameworkCodeRepository.Object, _mockStandardRepository.Object);
        }
    }
}
