using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingContactNumber
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, null, false)
                    .SetName("And is not provided then is valid."),
                new TestCaseData("abcdcom<>", "'Contact Number' is invalid.", true)
                    .SetName("And has invalid characters then is invalid."),
                new TestCaseData(
                        new string('1', 17),
                        "'Contact Number' must be less than 17 characters. You entered 17 characters.",
                        true)
                    .SetName("And if length exceeds 17 then is invalid."),
                new TestCaseData("+44 (0123)-45678", null, false)
                    .SetName("And contains allowed characters and numbers then is invalid.")
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateContactEmail(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest()
            {
                ContactNumber = value
            };

            var sut = new CreateApprenticeshipRequestValidator();

            if (shouldError)
            {
                var s = sut.Validate(request);
                sut.ShouldHaveValidationErrorFor(req => req.ContactNumber, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactNumber)
                    .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.ContactNumber, request);
            }
        }
    }
}