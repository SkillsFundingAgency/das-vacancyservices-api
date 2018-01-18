using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndLocationTypeOfOther
{
    public class WhenValidatingPostcode
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, false, null, "31048").SetName("And is null Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 10),  "31049").SetName("And exceeds 9 characters Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", "31049").SetName("And contains illegal chars Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, true, "CV1 2WT", null).SetName("And is in allowed format Then is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidatePostcode(LocationType locationType, bool isValid, string postCode, string errorCode)
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
                validator
                    .ShouldHaveValidationErrorFor(r => r.Postcode, request)
                    .WithErrorCode(errorCode);
            }
        }
    }
}