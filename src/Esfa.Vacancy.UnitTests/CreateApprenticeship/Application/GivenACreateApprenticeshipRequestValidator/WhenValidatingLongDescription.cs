using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Validation;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingLongDescription : CreateApprenticeshipRequestValidatorBase
    {

        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, false, ErrorCodes.CreateApprenticeship.LongDescriptionIsRequired, "'Long Description' should not be empty.")
                    .SetName("LongDescription cannot be null"),
                new TestCaseData("", false, ErrorCodes.CreateApprenticeship.LongDescriptionIsRequired, "'Long Description' should not be empty.")
                    .SetName("LongDescription cannot be empty"),
                new TestCaseData("<", true, ErrorCodes.CreateApprenticeship.LongDescriptionShouldNotIncludeSpecialCharacters, "'LongDescription' can't contain invalid characters") 
                    .SetName("LongDescription should contain valid characters"),
                new TestCaseData("~", false, ErrorCodes.CreateApprenticeship.LongDescriptionShouldNotIncludeSpecialCharacters, "'LongDescription' can't contain invalid characters")
                    .SetName("LongDescription should contain valid characters"),
                new TestCaseData("< i n p u t >", false, ErrorCodes.CreateApprenticeship.LongDescriptionShouldNotIncludeBlacklistedHtmlElements, "'LongDescription' can't contain blacklisted HTML elements")
                    .SetName("LongDescription cannot contain <input>"),
                new TestCaseData("< o b j e c t >", false, ErrorCodes.CreateApprenticeship.LongDescriptionShouldNotIncludeBlacklistedHtmlElements, "'LongDescription' can't contain blacklisted HTML elements")
                    .SetName("LongDescription cannot contain <object>"),
                new TestCaseData("< s c r i p t >", false, ErrorCodes.CreateApprenticeship.LongDescriptionShouldNotIncludeBlacklistedHtmlElements, "'LongDescription' can't contain blacklisted HTML elements")
                    .SetName("LongDescription cannot contain <script>"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ThenShouldValidate(string longDescription, bool isValid, string errorCode, string errorMessage)
        {
            var sut = new CreateApprenticeshipRequestValidator();

            var request = new CreateApprenticeshipRequest()
            {
                LongDescription = longDescription
            };

            //only validate LongDescription
            var context = GetValidationContextForProperty(request, "LongDescription");

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
