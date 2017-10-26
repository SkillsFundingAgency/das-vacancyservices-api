using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenSearchApprenticeshipVacanciesRequestValidator
{
    public abstract class GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        internal Mock<IFrameworkCodeRepository> FrameworkCodeRepositoryMock;
        internal static List<string> ValidFrameworkCodes = new List<string> { "1" };
        internal Mock<IStandardRepository> StandardRepositoryMock;
        internal static List<string> ValidStandardCodes = new List<string>() { "1" };
        internal SearchApprenticeshipVacanciesRequestValidator Validator { get; private set; }

        [SetUp]
        public void Setup()
        {
            StandardRepositoryMock = new Mock<IStandardRepository>();
            StandardRepositoryMock.Setup(r => r.GetStandardIdsAsync()).ReturnsAsync(ValidStandardCodes.Select(int.Parse));
            FrameworkCodeRepositoryMock = new Mock<IFrameworkCodeRepository>();
            FrameworkCodeRepositoryMock.Setup(r => r.GetAsync()).ReturnsAsync(ValidFrameworkCodes);
            Validator = new SearchApprenticeshipVacanciesRequestValidator(
                FrameworkCodeRepositoryMock.Object, StandardRepositoryMock.Object);
        }


    }
}
