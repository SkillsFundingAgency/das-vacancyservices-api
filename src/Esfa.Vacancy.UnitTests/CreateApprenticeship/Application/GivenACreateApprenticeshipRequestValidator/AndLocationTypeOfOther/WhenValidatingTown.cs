using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndLocationTypeOfOther
{
    [TestFixture]
    public class WhenValidatingTown
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, false, null, "31045").SetName("Then it is required"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 101),  "31046").SetName("Then cannot exceed 100 characters"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>", "31047").SetName("Then can include free text white list characters only"),
                new TestCaseData(LocationType.OtherLocation, true, "Coventry", null).SetName("Then it is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAddressLine1(LocationType locationType, bool isValid, string town, string errorCode)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = locationType,
                Town = town
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (isValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.Town, request);
            }
            else
            {
                validator
                    .ShouldHaveValidationErrorFor(r => r.Town, request)
                    .WithErrorCode(errorCode);
            }
        }
    }
}