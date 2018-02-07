using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    using Domain.Validation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Vacancy.Application.Commands.CreateApprenticeship;
    using Vacancy.Application.Commands.CreateApprenticeship.Validators;

    public class WhenValidatingFutureProspects
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, "'Future Prospects' should not be empty.", true)
                    .SetName("Then FutureProspects cannot be null"),
                new TestCaseData("", "'Future Prospects' should not be empty.", true)
                    .SetName("Then FutureProspects cannot be empty"),
                new TestCaseData("future prospects", null, false)
                    .SetName("Then FutureProspects is not empty"),
                new TestCaseData(new String('a', 4001),
                        "'Future Prospects' must be less than 4001 characters. You entered 4001 characters.", true)
                    .SetName("Then FutureProspects cannot be more than 4000 characters"),
                new TestCaseData("<", "'Future Prospects' can't contain invalid characters", true)
                    .SetName("Then FutureProspects cannot contain invalid characters"),
                new TestCaseData("<script>", "'Future Prospects' can't contain blacklisted HTML elements", true)
                    .SetName("Then FutureProspectsDe cannot contain blacklisted HTML elements")
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateFutureProspects(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                FutureProspects = value
            };

            var sut = new CreateApprenticeshipRequestValidator();
            if (shouldError)
            {
                sut.Validate(request);
                sut.ShouldHaveValidationErrorFor(req => req.FutureProspects, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.FutureProspects)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.FutureProspects, request);
            }
        }
    }
}
