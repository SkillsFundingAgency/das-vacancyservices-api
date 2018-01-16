using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndLocationTypeOfOther
{
    public class WhenValidatingPostcode
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, false, null, "31048").SetName("Then it is required"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 10),  "31049").SetName("Then cannot exceed 9 characters"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", "31049").SetName("Then can include free text white list characters only"),
                new TestCaseData(LocationType.OtherLocation, true, "CV1 2WT", null).SetName("Then it is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAddressLine1(LocationType locationType, bool isValid, string PostCode, string errorCode)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = locationType,
                Postcode = PostCode
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (isValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.Postcode, request);
            }
            else
            {
                validator
                    .ShouldHaveValidationErrorFor(r => r.Postcode, request)
                    .WithErrorCode(errorCode);
            }
        }
    }
}