using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.WhenValidatingLocation.AndLocationTypeOfOther
{
    [TestFixture]
    public class WhenValidatingTown
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, false, null)
                    .SetName("And is null Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 101))
                    .SetName("And exceeds 100 characters Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>")
                    .SetName("And contains illegal chars Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, true, "Coventry")
                    .SetName("And is in allowed format Then is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateTown(LocationType locationType, bool isValid, string town)
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
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.Town);
            }
        }
    }
}