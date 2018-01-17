using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndLocationTypeOfOther
{
    [TestFixture]
    public class WhenValidatingAddressLine1
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, false, null, "31032").SetName("And is null Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 301),  "31033").SetName("And exceeds 300 characters Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", "31034").SetName("And contains illegal chars Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, true, "10 Downing Street", null).SetName("And is in allowed format Then is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAddressLine1(LocationType locationType, bool isValid, string addressLine1, string errorCode)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = locationType,
                AddressLine1 = addressLine1
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (isValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.AddressLine1, request);
            }
            else
            {
                validator
                    .ShouldHaveValidationErrorFor(r => r.AddressLine1, request)
                    .WithErrorCode(errorCode);
            }
        }
    }
}