using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.
    WhenValidatingExpectedDuration
{
    [TestFixture]
    public class AndDurationTypeIsWeeks
    {
        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(null, "'Expected Duration' should not be empty.", true)
                .SetName("Then null is invalid"),
            new TestCaseData(51, "'Expected Duration' must be greater than or equal to '52'.", true)
                .SetName("Then 51 weeks is invalid"),
            new TestCaseData(52, null, false)
                .SetName("Then 52 weeks is valid"),
            new TestCaseData(53, null, false)
                .SetName("Then 53 weeks is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateDurationTypeWeeks(int value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                ExpectedDuration = value,
                DurationType = DurationType.Weeks
            };
            var sut = new Fixture().Customize(new AutoMoqCustomization())
                                   .Create<CreateApprenticeshipRequestValidator>();

            sut.Validate(request);

            if (shouldError)
            {
                sut.ShouldHaveValidationErrorFor(req => req.ExpectedDuration, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedDuration)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.ExpectedDuration, request);
            }
        }
    }
}
