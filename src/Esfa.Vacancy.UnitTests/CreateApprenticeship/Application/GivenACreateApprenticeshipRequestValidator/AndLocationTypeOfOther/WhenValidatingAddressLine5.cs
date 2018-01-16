using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndLocationTypeOfOther
{
    [TestFixture]
    public class WhenValidatingAddressLine5
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, true, null, null).SetName("Then it is not required"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 301),  "31043").SetName("Then cannot exceed 300 characters"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", "31044").SetName("Then can include free text white list characters only"),
                new TestCaseData(LocationType.OtherLocation, true, "10 Downing Street", null).SetName("Then it is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAddressLine5(LocationType locationType, bool isValid, string addressLine5, string errorCode)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = locationType,
                AddressLine5 = addressLine5
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (isValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.AddressLine5, request);
            }
            else
            {
                validator
                    .ShouldHaveValidationErrorFor(r => r.AddressLine5, request)
                    .WithErrorCode(errorCode);
            }
        }
    }
}