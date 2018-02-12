using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.WhenValidatingLocation.AndLocationTypeOfOther
{
    [TestFixture]
    public class WhenValidatingAddressLine2
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, true, null)
                    .SetName("And is null Then is valid"),
                new TestCaseData(string.Empty, true, null)
                    .SetName("And is empty Then is valid"),
                new TestCaseData("   ", true, null)
                    .SetName("And is whitespace Then is valid"),
                new TestCaseData(new string('a', 301), false, "'Address Line2' must be less than 301 characters. You entered 301 characters.")
                    .SetName("And exceeds 300 characters Then is invalid"),
                new TestCaseData("<p>", false, "'Address Line2' can't contain invalid characters")
                    .SetName("And contains illegal chars Then is invalid"),
                new TestCaseData("10 Downing Street", true)
                    .SetName("And is in allowed format Then is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAddressLine2(string addressLine2, bool isValid, string errorMessage)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = LocationType.OtherLocation,
                AddressLine2 = addressLine2
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (isValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.AddressLine2, request);
            }
            else
            {
                validator
                    .ShouldHaveValidationErrorFor(r => r.AddressLine2, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2)
                    .WithErrorMessage(errorMessage);
            }
        }
    }
}