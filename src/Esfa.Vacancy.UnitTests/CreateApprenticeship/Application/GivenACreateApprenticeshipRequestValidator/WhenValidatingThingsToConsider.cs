using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    public class WhenValidatingThingsToConsider
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, null, false)
                    .SetName("Then ThingsToConsider can be null"),
                new TestCaseData("", null, false)
                    .SetName("Then ThingsToConsider can be empty"),
                new TestCaseData("future prospects", null, false)
                    .SetName("Then ThingsToConsider is not empty"),
                new TestCaseData(new String('a', 4001),
                        "'Things To Consider' must be less than 4001 characters. You entered 4001 characters.", true)
                    .SetName("Then ThingsToConsider cannot be more than 4000 characters"),
                new TestCaseData("<", null, false)
                    .SetName("Then ThingsToConsider can contain valid characters"),
                new TestCaseData("<script>", "'Things To Consider' can't contain blacklisted HTML elements", true)
                    .SetName("Then ThingsToConsider cannot contain blacklisted HTML elements")
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateThingsToConsider(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                ThingsToConsider = value
            };

            var sut = new CreateApprenticeshipRequestValidator();
            if (shouldError)
            {
                sut.Validate(request);
                sut.ShouldHaveValidationErrorFor(req => req.ThingsToConsider, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.ThingsToConsider)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.ThingsToConsider, request);
            }
        }
    }
}
