namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    using System;
    using System.Collections.Generic;
    using Domain.Validation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Vacancy.Application.Commands.CreateApprenticeship;
    using Vacancy.Application.Commands.CreateApprenticeship.Validators;

    [TestFixture]
    public class WhenValidatingDesiredSkills
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, "'Desired Skills' should not be empty.", true)
                    .SetName("Then DesiredSkills cannot be null"),
                new TestCaseData("", "'Desired Skills' should not be empty.", true)
                    .SetName("Then DesiredSkills cannot be empty"),
                new TestCaseData("desired skill", null, false)
                    .SetName("Then DesiredSkills is not empty"),
                new TestCaseData(new String('a', 4001),"'Desired Skills' must be less than 4001 characters. You entered 4001 characters.", true)
                    .SetName("Then DesiredSkills cannot be more than 4000 characters"),
                new TestCaseData("<","'Desired Skills' can't contain invalid characters", true)
                    .SetName("Then DesiredSkills cannot contain invalid characters"),
                new TestCaseData("<script>", "'Desired Skills' can't contain blacklisted HTML elements", true)
                    .SetName("Then DesiredSkills cannot contain blacklisted HTML elements")
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateDesiredSkills(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                DesiredSkills = value
            };

            var sut = new CreateApprenticeshipRequestValidator();
            if (shouldError)
            {
                sut.Validate(request);
                sut.ShouldHaveValidationErrorFor(req => req.DesiredSkills, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredSkills)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.DesiredSkills, request);
            }
        }
    }
}
