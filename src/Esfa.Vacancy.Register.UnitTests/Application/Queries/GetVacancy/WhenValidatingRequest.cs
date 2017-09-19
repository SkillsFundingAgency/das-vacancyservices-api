using System;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.Application.Queries.GetVacancy
{
    [TestFixture, Ignore("temporary")]
    public class WhenValidatingRequest
    {
        private GetVacancyValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new GetVacancyValidator();
        }

        [TestCase("0")]
        [TestCase("-1")]
        public void ThenIfTheVacancyReferenceIsZeroOrLessIsNotValid(string testVacancyReference)
        {
            _validator.ShouldHaveValidationErrorFor(request => request.Reference, testVacancyReference);
        }

        [TestCase(1)]
        [TestCase(99999)]
        public void ThenIfTheVacancyReferenceIsGreaterThanZeroIsValid(int testVacancyReference)
        {
            _validator.ShouldNotHaveValidationErrorFor(request => int.Parse(request.Reference), testVacancyReference);
        }
    }
}
