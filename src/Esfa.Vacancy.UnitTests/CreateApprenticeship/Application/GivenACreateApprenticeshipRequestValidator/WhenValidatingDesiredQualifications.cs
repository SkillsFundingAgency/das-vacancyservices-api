using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
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
                new TestCaseData("<", null, false)
                    .SetName("Then DesiredQualifications can contain valid characters"),
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

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

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
