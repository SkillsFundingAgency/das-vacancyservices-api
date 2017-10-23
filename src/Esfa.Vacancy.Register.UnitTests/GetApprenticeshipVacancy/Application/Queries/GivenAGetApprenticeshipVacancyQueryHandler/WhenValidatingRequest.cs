using Esfa.Vacancy.Register.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Register.Domain;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.GetApprenticeshipVacancy.Application.Queries.GivenAGetApprenticeshipVacancyQueryHandler
{
    [TestFixture]
    public class WhenValidatingRequest
    {
        private GetApprenticeshipVacancyValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new GetApprenticeshipVacancyValidator();
        }

        [TestCase(0, ErrorCodes.GetApprenticeship.VacancyReferenceNumberLessThan0)]
        [TestCase(-1, ErrorCodes.GetApprenticeship.VacancyReferenceNumberLessThan0)]
        public void ThenIfTheVacancyReferenceIsZeroOrLessIsNotValid(int testVacancyReference, int errorCode)
        {
            _validator.ShouldHaveValidationErrorFor(request => request.Reference, testVacancyReference)
                .WithErrorCode(errorCode.ToString());
        }

        [TestCase(1)]
        [TestCase(99999)]
        public void ThenIfTheVacancyReferenceIsGreaterThanZeroIsValid(int testVacancyReference)
        {
            _validator.ShouldNotHaveValidationErrorFor(request => request.Reference, testVacancyReference);
        }
    }
}
