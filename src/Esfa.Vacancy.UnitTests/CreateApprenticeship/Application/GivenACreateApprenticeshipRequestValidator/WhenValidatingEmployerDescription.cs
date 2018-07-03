using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingEmployerDescription : CreateApprenticeshipRequestValidatorBase
    {
        private static List<TestCaseData> FailingTestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(new string('a', 4001),
                        "'Employer Description' must be less than 4001 characters. You entered 4001 characters.")
                    .SetName("Should contain 4000 or less characters"),
                new TestCaseData("<",
                    "'Employer Description' can't contain invalid characters")
                    .SetName("Should contain valid characters")
            };

        [TestCaseSource(nameof(FailingTestCases))]
        public void ThenShouldValidate(string employerDescription, string errorMessage)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

            var request = new CreateApprenticeshipRequest()
            {
                EmployerDescription = employerDescription
            };

            var context = GetValidationContextForProperty(request, req => req.EmployerDescription);

            var result = sut.Validate(context);

            Assert.AreEqual(false, result.IsValid);

            Assert.AreEqual(ErrorCodes.CreateApprenticeship.EmployerDescription, result.Errors[0].ErrorCode);
            Assert.AreEqual(errorMessage, result.Errors[0].ErrorMessage);
        }
    }
}