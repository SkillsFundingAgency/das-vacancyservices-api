using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndLocationTypeOfOther
{
    [TestFixture]
    public class WhenValidatingAddressLine2
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, false, null, "31035").SetName("Then it is required"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 301),  "31036").SetName("Then cannot exceed 300 characters"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", "31037").SetName("Then can include free text white list characters only"),
                new TestCaseData(LocationType.OtherLocation, true, "10 Downing Street", null).SetName("Then it is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAddressLine2(LocationType locationType, bool isValid, string addressLine2, string errorCode)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = locationType,
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
                    .WithErrorCode(errorCode);
            }
        }
    }
}