using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndLocationTypeOfOther
{
    [TestFixture]
    public class WhenValidatingAddressLine4
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, true, null, null).SetName("Then it is not required"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 301),  "31041").SetName("Then cannot exceed 300 characters"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", "31042").SetName("Then can include free text white list characters only"),
                new TestCaseData(LocationType.OtherLocation, true, "10 Downing Street", null).SetName("Then it is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAddressLine4(LocationType locationType, bool isValid, string addressLine4, string errorCode)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = locationType,
                AddressLine4 = addressLine4
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (isValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.AddressLine4, request);
            }
            else
            {
                validator
                    .ShouldHaveValidationErrorFor(r => r.AddressLine4, request)
                    .WithErrorCode(errorCode);
            }
        }
    }
}