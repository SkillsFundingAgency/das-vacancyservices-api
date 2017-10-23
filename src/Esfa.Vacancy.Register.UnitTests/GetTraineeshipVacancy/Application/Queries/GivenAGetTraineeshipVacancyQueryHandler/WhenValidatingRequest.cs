using Esfa.Vacancy.Register.Application.Queries.GetTraineeshipVacancy;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.GetTraineeshipVacancy.Application.Queries.GivenAGetTraineeshipVacancyQueryHandler
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

        [TestCase(0)]
        [TestCase(-1)]
        public void ThenIfTheVacancyReferenceIsZeroOrLessIsNotValid(int testVacancyReference)
        {
            _validator.ShouldHaveValidationErrorFor(request => request.Reference, testVacancyReference);
        }

        [TestCase(1)]
        [TestCase(99999)]
        public void ThenIfTheVacancyReferenceIsGreaterThanZeroIsValid(int testVacancyReference)
        {
            _validator.ShouldNotHaveValidationErrorFor(request => request.Reference, testVacancyReference);
        }
    }
}
