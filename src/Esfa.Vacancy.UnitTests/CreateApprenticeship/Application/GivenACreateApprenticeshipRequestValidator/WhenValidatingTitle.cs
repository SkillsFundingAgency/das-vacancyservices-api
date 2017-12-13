using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using NUnit.Framework;
using NUnit.Framework.Internal;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingTitle
    {
        private static List<TestCaseData> FailingTestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, TitleIsRequired)
                    .SetName("Title cannot be null"),
                new TestCaseData("title", TitleShouldIncludeWordApprentice)
                    .SetName("Fail if title does not contain word 'apprentice'"),
                new TestCaseData("1apprentice0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
                    TitleMaximumFieldLength)
                    .SetName("Should contain 100 or less characters")
            };

        [TestCaseSource(nameof(FailingTestCases))]
        public void ThenCheckFailingCases(string title, string errorCode)
        {
            var sut = new CreateApprenticeshipRequestValidator();

            var request = new CreateApprenticeshipRequest()
            {
                Title = title
            };

            var result = sut.Validate(request);

            Assert.AreEqual(false, result.IsValid);

            Assert.AreEqual(errorCode, result.Errors[0].ErrorCode);
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

            var result = sut.Validate(request);

            Assert.AreEqual(true, result.IsValid);
        }
    }
}