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
                new TestCaseData(LocationType.OtherLocation, false, null, "31032").SetName("Then it is required"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 301),  "31033").SetName("Then cannot exceed 300 characters"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", "31034").SetName("Then can include free text white list characters only"),
                new TestCaseData(LocationType.OtherLocation, true, "10 Downing Street", null).SetName("Then it is valid"),
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