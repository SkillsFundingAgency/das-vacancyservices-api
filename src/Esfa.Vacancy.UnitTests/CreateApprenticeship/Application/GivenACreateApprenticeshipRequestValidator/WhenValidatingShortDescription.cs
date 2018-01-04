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
    public class WhenValidatingShortDescription : CreateApprenticeshipRequestValidatorBase
    {

        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, false, ShortDescriptionIsRequired)
                    .SetName("ShortDescription cannot be null"),
                new TestCaseData("", false, ShortDescriptionIsRequired)
                    .SetName("ShortDescription cannot be empty"),
                new TestCaseData(new string('a', 351), false, ShortDescriptionMaximumFieldLength)
                    .SetName("ShortDescription should contain 350 or less characters"),
                new TestCaseData(new string('a', 350), true, null)
                    .SetName("ShortDescription contains 350 or less characters"),
                new TestCaseData("<", false, ShortDescriptionShouldNotIncludeSpecialCharacters)
                    .SetName("ShortDescription should contain valid characters"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ThenShouldValidate(string shortDescription, bool isValid, string errorCode)
        {
            var sut = new CreateApprenticeshipRequestValidator();

            var request = new CreateApprenticeshipRequest()
            {
                ShortDescription = shortDescription
            };

            //only validate ShortDescription
            var context = GetValidationContextForProperty(request, "ShortDescription");

            var result = sut.Validate(context);

            Assert.AreEqual(isValid, result.IsValid);

            if (!result.IsValid)
            {
                Assert.AreEqual(errorCode, result.Errors[0].ErrorCode);
            }
        }
    }
}
