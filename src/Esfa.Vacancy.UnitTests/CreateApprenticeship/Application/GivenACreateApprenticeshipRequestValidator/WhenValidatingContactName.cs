﻿using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingContactName
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, null, false)
                    .SetName("And is null then is valid."),
                new TestCaseData(string.Empty, "'Contact Name' is invalid.", true)
                    .SetName("And is empty string then is not valid."),
                new TestCaseData(
                        new string('a', 101),
                        "'Contact Name' must be less than 101 characters. You entered 101 characters.",
                        true)
                    .SetName("And if length exceeds 100 then is invalid."),
                new TestCaseData("{@<", "'Contact Name' is invalid.", true)
                    .SetName("And if contains special character then is invalid."),
                new TestCaseData("122134", "'Contact Name' is invalid.", true)
                    .SetName("And if contains numbers then is invalid.")
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateContactDetails(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest()
            {
                ContactName = value
            };

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

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