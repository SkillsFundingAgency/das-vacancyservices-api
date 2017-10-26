using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenSearchApprenticeshipVacanciesRequestValidator
{
    public abstract class GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        internal Mock<IFrameworkCodeRepository> FrameworkCodeRepositoryMock;

        internal SearchApprenticeshipVacanciesRequestValidator Validator { get; private set; }

        internal static string[] ValidFrameworkCodes = { "1" };

        [SetUp]
        public void Setup()
        {
            FrameworkCodeRepositoryMock = new Mock<IFrameworkCodeRepository>();
            FrameworkCodeRepositoryMock.Setup(r => r.GetAsync()).ReturnsAsync(ValidFrameworkCodes);
            Validator = new SearchApprenticeshipVacanciesRequestValidator(FrameworkCodeRepositoryMock.Object);
        }
    }
}
