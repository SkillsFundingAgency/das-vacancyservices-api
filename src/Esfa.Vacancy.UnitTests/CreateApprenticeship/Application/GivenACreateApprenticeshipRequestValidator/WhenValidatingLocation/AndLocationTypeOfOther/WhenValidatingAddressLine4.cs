﻿using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.WhenValidatingLocation.AndLocationTypeOfOther
{
    [TestFixture]
    public class WhenValidatingAddressLine4
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(LocationType.OtherLocation, true, null)
                    .SetName("And is null Then is valid"),
                new TestCaseData(LocationType.OtherLocation, false, new string('a', 301))
                    .SetName("And exceeds 300 characters Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, false, "<p>")
                    .SetName("And contains illegal chars Then is invalid"),
                new TestCaseData(LocationType.OtherLocation, true, "10 Downing Street")
                    .SetName("And is in allowed format Then is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAddressLine4(LocationType locationType, bool isValid, string addressLine4)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = locationType,
                AddressLine4 = addressLine4
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (isValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.AddressLine4, request);
            }
            else
            {
                validator
                    .ShouldHaveValidationErrorFor(r => r.AddressLine4, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine4);
            }
        }
    }
}