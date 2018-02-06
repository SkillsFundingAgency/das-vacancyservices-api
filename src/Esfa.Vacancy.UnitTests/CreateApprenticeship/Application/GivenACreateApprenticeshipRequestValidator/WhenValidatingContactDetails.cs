﻿using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingContactDetails
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, null, false)
                    .SetName("And is not provided then is valid."),
                new TestCaseData(new string('a', 101), "'Contact Name' must be less than 101 characters. You entered 101 characters." , true)
                    .SetName("And if length exceeds 100 then is invalid."),
                new TestCaseData("<", "'Contact Name' can't contain invalid characters", true)
                .SetName("And if contains special character then is invalid.")
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateContactDetails(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest()
            {
                ContactName = value
            };

            var sut = new CreateApprenticeshipRequestValidator();

            if (shouldError)
            {
                sut.ShouldHaveValidationErrorFor(req => req.ContactName, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactName)
                    .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.ContactName, request);
            }
        }
    }
}