using Esfa.Vacancy.Register.Application.Queries.GetTraineeshipVacancy;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.GetTraineeshipVacancy.Application.Queries.GivenAGetTraineeshipVacancyValidator
{
    [TestFixture]
    public class WhenValidatingRequest
    {
        private GetTraineeshipVacancyValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new GetTraineeshipVacancyValidator();
        }

        [TestCase(0, ErrorCodes.GetTraineeship.VacancyReferenceNumberLessThan0)]
        [TestCase(-1, ErrorCodes.GetTraineeship.VacancyReferenceNumberLessThan0)]
        public void AndVacancyReferenceIsZeroOrLess_ThenValidationFails(int testVacancyReference, string errorCode)
        {
            _validator
                .ShouldHaveValidationErrorFor(request => request.Reference, testVacancyReference)
                .WithErrorCode(errorCode);
        }

        [TestCase(1)]
        [TestCase(99999)]
        public void AndVacancyReferenceIsGreaterThanZero_ThenValidationPasses(int testVacancyReference)
        {
            _validator.ShouldNotHaveValidationErrorFor(request => request.Reference, testVacancyReference);
        }
    }
}
