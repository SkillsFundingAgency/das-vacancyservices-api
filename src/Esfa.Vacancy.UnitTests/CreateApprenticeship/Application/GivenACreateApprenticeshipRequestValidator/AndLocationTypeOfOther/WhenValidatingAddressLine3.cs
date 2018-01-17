using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndLocationTypeOfOther
{
    [TestFixture]
    public class WhenValidatingAddressLine3
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, false, null, "31038").SetName("And is null Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 301),  "31039").SetName("And exceeds 300 characters Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", "31040").SetName("And contains illegal chars Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, true, "10 Downing Street", null).SetName("And is in allowed format Then is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAddressLine3(LocationType locationType, bool isValid, string addressLine3, string errorCode)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = locationType,
                AddressLine3 = addressLine3
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (isValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.AddressLine3, request);
            }
            else
            {
                validator
                    .ShouldHaveValidationErrorFor(r => r.AddressLine3, request)
                    .WithErrorCode(errorCode);
            }
        }
    }
}