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
                new TestCaseData("<", null, false)
                    .SetName("Then FutureProspects can contain valid characters"),
                new TestCaseData("<script>", "'Future Prospects' can't contain blacklisted HTML elements", true)
                    .SetName("Then FutureProspects cannot contain blacklisted HTML elements")
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateFutureProspects(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                FutureProspects = value
            };

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

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
