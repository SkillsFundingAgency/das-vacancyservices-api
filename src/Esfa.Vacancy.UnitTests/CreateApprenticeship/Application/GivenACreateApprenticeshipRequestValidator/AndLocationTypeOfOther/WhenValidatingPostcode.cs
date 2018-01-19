using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndLocationTypeOfOther
{
    public class WhenValidatingPostcode
    {
        private const string EmptyMessage = "'Postcode' should not be empty.";
        private const string InvalidMessage = "'Postcode' is invalid.";
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, false, null, "31048", EmptyMessage)
                    .SetName("And is null Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 10),  "31049", InvalidMessage)
                    .SetName("And exceeds 9 characters Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", "31049", InvalidMessage)
                    .SetName("And contains illegal chars Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, "  ", "31048", EmptyMessage)
                    .SetName("And is whitespaces Then raise not empty validation error only"),
                new TestCaseData(LocationType.OtherLocation, true, "CV1 2WT", null, string.Empty)
                    .SetName("And is in allowed format Then is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidatePostcode(LocationType locationType, bool isValid, string postCode, string errorCode, string errorMessage)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = locationType,
                Postcode = postCode
            };

            var validator = new CreateApprenticeshipRequestValidator();
			
            if (isValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.Postcode, request);
            }
            else
            {
                var s = validator
                    .ShouldHaveValidationErrorFor(r => r.Postcode, request)
                    .WithErrorCode(errorCode)
                    .WithErrorMessage(errorMessage);
                s.Count().Should().Be(1);
            }
        }
    }
}