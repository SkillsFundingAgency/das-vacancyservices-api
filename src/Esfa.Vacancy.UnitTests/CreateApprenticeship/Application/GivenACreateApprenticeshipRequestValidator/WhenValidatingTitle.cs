using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingTitle : CreateApprenticeshipRequestValidatorBase
    {
        private static List<TestCaseData> FailingTestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, ErrorCodes.CreateApprenticeship.TitleIsRequired, "'Title' should not be empty.")
                    .SetName("Title cannot be null"),
                new TestCaseData("title", ErrorCodes.CreateApprenticeship.TitleShouldIncludeWordApprentice, ErrorMessages.CreateApprenticeship.TitleShouldIncludeWordApprentice)
                    .SetName("Fail if title does not contain word 'apprentice'"),
                new TestCaseData("apprentice0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
                        ErrorCodes.CreateApprenticeship.TitleMaximumFieldLength, "'Title' must be less than 101 characters. You entered 101 characters.")
                    .SetName("Should contain 100 or less characters"),
                new TestCaseData("apprentice <", ErrorCodes.CreateApprenticeship.TitleShouldNotIncludeSpecialCharacters, "'Title' can't contain invalid characters")
                    .SetName("Should contain valid characters")
            };

        [TestCaseSource(nameof(FailingTestCases))]
        public void ThenCheckFailingCases(string title, string errorCode, string errorMessage)
        {
            var sut = new CreateApprenticeshipRequestValidator();

            var request = new CreateApprenticeshipRequest()
            {
                Title = title
            };

            //only validate Title
            var context = GetValidationContextForProperty(request, req => req.Title);

            var result = sut.Validate(context);

            Assert.AreEqual(false, result.IsValid);

            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(errorCode, result.Errors[0].ErrorCode);
            Assert.AreEqual(errorMessage, result.Errors[0].ErrorMessage);
        }


        private static List<TestCaseData> PassingTestCases =>
            new List<TestCaseData>
            {
                new TestCaseData("title APPRENTICEship")
                    .SetName("Case or word combo does not matter"),
                new TestCaseData("apprentice012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789")
                    .SetName("100 characters is allowed")
            };

        [TestCaseSource(nameof(PassingTestCases))]
        public void ThenCheckAllowedCases(string title)
        {
            
            var sut = new CreateApprenticeshipRequestValidator();

            var request = new CreateApprenticeshipRequest()
            {
                Title = title
            };

            //only validate Title
            var context = GetValidationContextForProperty(request, req => req.Title);

            var result = sut.Validate(context);

            Assert.AreEqual(true, result.IsValid);
        }
        
    }
}