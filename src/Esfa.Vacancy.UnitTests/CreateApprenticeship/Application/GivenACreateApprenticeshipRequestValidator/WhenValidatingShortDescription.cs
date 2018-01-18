using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Validation;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingShortDescription : CreateApprenticeshipRequestValidatorBase
    {

        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, false, ErrorCodes.CreateApprenticeship.ShortDescriptionIsRequired, "'Short Description' should not be empty.")
                    .SetName("ShortDescription cannot be null"),
                new TestCaseData("", false, ErrorCodes.CreateApprenticeship.ShortDescriptionIsRequired, "'Short Description' should not be empty.")
                    .SetName("ShortDescription cannot be empty"),
                new TestCaseData(new string('a', 351), false, ErrorCodes.CreateApprenticeship.ShortDescriptionMaximumFieldLength, "'Short Description' must be less than 351 characters. You entered 351 characters.")
                    .SetName("ShortDescription should contain 350 or less characters"),
                new TestCaseData(new string('a', 350), true, null, null)
                    .SetName("ShortDescription contains 350 or less characters"),
                new TestCaseData("<", false, ErrorCodes.CreateApprenticeship.ShortDescriptionShouldNotIncludeSpecialCharacters, "'Short Description' can't contain invalid characters")
                    .SetName("ShortDescription should contain valid characters"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ThenShouldValidate(string shortDescription, bool isValid, string errorCode, string errorMessage)
        {
            var sut = new CreateApprenticeshipRequestValidator();

            var request = new CreateApprenticeshipRequest()
            {
                ShortDescription = shortDescription
            };

            //only validate ShortDescription
            var context = GetValidationContextForProperty(request, req => req.ShortDescription);

            var result = sut.Validate(context);

            Assert.AreEqual(isValid, result.IsValid);

            if (!result.IsValid)
            {
                Assert.AreEqual(1, result.Errors.Count);
                Assert.AreEqual(errorCode, result.Errors[0].ErrorCode);
                Assert.AreEqual(errorMessage, result.Errors[0].ErrorMessage);
            }
        }
    }
}
