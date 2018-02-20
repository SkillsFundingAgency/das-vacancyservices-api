using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingContactNumber
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, null, false)
                    .SetName("And is null then is valid."),
                new TestCaseData(string.Empty, "'Contact Number' is invalid.", true)
                    .SetName("And is empty string then is not valid."),
                new TestCaseData("abcdcom<>", "'Contact Number' is invalid.", true)
                    .SetName("And has invalid characters then is invalid."),
                new TestCaseData(new string('1', 17), "'Contact Number' is invalid.", true)
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

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

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