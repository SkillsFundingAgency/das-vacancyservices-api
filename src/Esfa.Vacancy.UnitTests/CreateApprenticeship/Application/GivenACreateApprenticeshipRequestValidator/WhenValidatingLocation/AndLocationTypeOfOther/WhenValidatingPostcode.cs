using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.WhenValidatingLocation.AndLocationTypeOfOther
{
    public class WhenValidatingPostcode
    {
        private const string EmptyMessage = "'Postcode' should not be empty.";
        private const string InvalidMessage = "'Postcode' is invalid.";
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, false, null, EmptyMessage)
                    .SetName("And is null Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 10), InvalidMessage)
                    .SetName("And exceeds 9 characters Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", InvalidMessage)
                    .SetName("And contains illegal chars Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, "  ", EmptyMessage)
                    .SetName("And is whitespaces Then raise not empty validation error only"),
                new TestCaseData(LocationType.OtherLocation, true, "CV1 2WT", string.Empty)
                    .SetName("And is in allowed format Then is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidatePostcode(LocationType locationType, bool isValid, string postCode, string errorMessage)
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
                var errors = validator
                    .ShouldHaveValidationErrorFor(r => r.Postcode, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.Postcode)
                    .WithErrorMessage(errorMessage);
                errors.Count().Should().Be(1);
            }
        }
    }
}