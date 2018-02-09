using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    public class WhenValidatingTrainingToBeProvided
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, "'Training To Be Provided' should not be empty.", true)
                    .SetName("Then TrainingToBeProvided cannot be null"),
                new TestCaseData("", "'Training To Be Provided' should not be empty.", true)
                    .SetName("Then TrainingToBeProvided cannot be empty"),
                new TestCaseData("training to be provided", null, false)
                    .SetName("Then TrainingToBeProvided is not empty"),
                new TestCaseData(new String('a', 4001),
                        "'Training To Be Provided' must be less than 4001 characters. You entered 4001 characters.", true)
                    .SetName("Then TrainingToBeProvided cannot be more than 4000 characters"),
                new TestCaseData("<", null, false)
                    .SetName("Then TrainingToBeProvided can contain valid free text characters"),
                new TestCaseData("<script>", "'Training To Be Provided' can't contain blacklisted HTML elements", true)
                    .SetName("Then TrainingToBeProvided cannot contain blacklisted HTML elements")
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateTrainingToBeProvided(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                TrainingToBeProvided = value
            };

            var sut = new CreateApprenticeshipRequestValidator();
            if (shouldError)
            {
                sut.Validate(request);
                sut.ShouldHaveValidationErrorFor(req => req.TrainingToBeProvided, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingToBeProvided)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.TrainingToBeProvided, request);
            }
        }
    }
}
