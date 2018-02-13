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
    public class WhenValidatingDurationType
    {
        private const int InvalidDurationType = 245;
        private const int ZeroDuration = 0;

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(null, "'Duration Type' should not be empty.", true)
                .SetName("Then null is invalid"),
            new TestCaseData((WageType)ZeroDuration, "'Duration Type' should not be empty.", true)
                .SetName("Then zero is invalid"),
            new TestCaseData((WageType)InvalidDurationType, "'Duration Type' has a range of values which does not include '245'.",
                    true)
                .SetName("Then an unknown duration type is invalid"),
            new TestCaseData(DurationType.Weeks, null, false).SetName("Then a known duration type is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateDurationType(DurationType value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                DurationType = value
            };
            var sut = new Fixture().Customize(new AutoMoqCustomization())
                                   .Create<CreateApprenticeshipRequestValidator>();

            sut.Validate(request);

            if (shouldError)
            {
                sut.ShouldHaveValidationErrorFor(req => req.DurationType, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.DurationType)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.DurationType, request);
            }
        }
    }
}
