using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentValidation;
using FluentValidation.Internal;
using NUnit.Framework;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingLongDescription : CreateApprenticeshipRequestValidatorBase
    {

        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, false, LongDescriptionIsRequired)
                    .SetName("LongDescription cannot be null"),
                new TestCaseData("", false, LongDescriptionIsRequired)
                    .SetName("LongDescription cannot be empty"),
                new TestCaseData("<", true, null)
                    .SetName("LongDescription should contain valid characters"),
                new TestCaseData("~", false, LongDescriptionShouldNotIncludeSpecialCharacters)
                    .SetName("LongDescription should contain valid characters"),
                new TestCaseData("< i n p u t >", false, LongDescriptionShouldNotIncludeBlacklistedHtmlElements)
                    .SetName("LongDescription should contain <input>"),
                new TestCaseData("< o b j e c t >", false, LongDescriptionShouldNotIncludeBlacklistedHtmlElements)
                    .SetName("LongDescription should contain <object>"),
                new TestCaseData("< s c r i p t >", false, LongDescriptionShouldNotIncludeBlacklistedHtmlElements)
                    .SetName("LongDescription should contain <script>"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ThenShouldValidate(string longDescription, bool isValid, string errorCode)
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
                Assert.AreEqual(errorCode, result.Errors[0].ErrorCode);
            }
        }
    }
}
