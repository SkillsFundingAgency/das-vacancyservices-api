namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    using System;
    using System.Collections.Generic;
    using Domain.Validation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Vacancy.Application.Commands.CreateApprenticeship;
    using Vacancy.Application.Commands.CreateApprenticeship.Validators;

    public class WhenValidatingDesiredQualifications
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, "'Desired Qualifications' should not be empty.", true)
                    .SetName("Then DesiredQualifications cannot be null"),
                new TestCaseData("", "'Desired Qualifications' should not be empty.", true)
                    .SetName("Then DesiredQualifications cannot be empty"),
                new TestCaseData("desired qualifications", null, false)
                    .SetName("Then DesiredQualifications is not empty"),
                new TestCaseData(new String('a', 4001),
                        "'Desired Qualifications' must be less than 4001 characters. You entered 4001 characters.", true)
                    .SetName("Then DesiredQualifications cannot be more than 4000 characters"),
                new TestCaseData("<", "'Desired Qualifications' can't contain invalid characters", true)
                    .SetName("Then DesiredQualifications cannot contain invalid characters"),
                new TestCaseData("<script>", "'Desired Qualifications' can't contain blacklisted HTML elements", true)
                    .SetName("Then DesiredQualifications cannot contain blacklisted HTML elements")
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateDesiredQualifications(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                DesiredQualifications = value
            };

            var sut = new CreateApprenticeshipRequestValidator();
            if (shouldError)
            {
                sut.Validate(request);
                sut.ShouldHaveValidationErrorFor(req => req.DesiredQualifications, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredQualifications)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.DesiredQualifications, request);
            }
        }
    }
}
