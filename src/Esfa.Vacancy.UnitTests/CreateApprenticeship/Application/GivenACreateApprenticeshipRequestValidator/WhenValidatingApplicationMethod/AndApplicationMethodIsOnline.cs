using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.
    WhenValidatingApplicationMethod
{
    [TestFixture]
    public class AndApplicationMethodIsOnline
    {
        private static List<TestCaseData> Question1TestCases() => new List<TestCaseData>
        {
            new TestCaseData(null, null, false)
                .SetName("Then SupplementaryQuestion1 can be null"),
            new TestCaseData(String.Empty, null, false)
                .SetName("Then SupplementaryQuestion1 can be empty"),
            new TestCaseData("supplementary question 1", null, false)
                .SetName("Then SupplementaryQuestion1 is valid"),
            new TestCaseData(new String('a', 4001),
                    "'Supplementary Question1' must be less than 4001 characters. You entered 4001 characters.", true)
                .SetName("Then SupplementaryQuestion1 cannot be more than 4000 characters"),
            new TestCaseData("<", "'Supplementary Question1' can't contain invalid characters", true)
                .SetName("Then SupplementaryQuestion1 cannot include invalid characters")
        };

        private static List<TestCaseData> Question2TestCases() => new List<TestCaseData>
        {
            new TestCaseData(null, null, false)
                .SetName("Then SupplementaryQuestion2 can be null"),
            new TestCaseData(String.Empty, null, false)
                .SetName("Then SupplementaryQuestion2 can be empty"),
            new TestCaseData("supplementary question 2", null, false)
                .SetName("Then SupplementaryQuestion2 is valid"),
            new TestCaseData(new String('a', 4001),
                    "'Supplementary Question2' must be less than 4001 characters. You entered 4001 characters.", true)
                .SetName("Then SupplementaryQuestion2 cannot be more than 4000 characters"),
            new TestCaseData("<", "'Supplementary Question2' can't contain invalid characters", true)
                .SetName("Then SupplementaryQuestion2 cannot include invalid characters")
        };

        private static List<TestCaseData> ExternalApplicationUrlCases() => new List<TestCaseData>
        {
            new TestCaseData(null, null, false)
                .SetName("Then a null ExternalApplicationUrl is valid"),
            new TestCaseData("", null, false)
                .SetName("Then an empty ExternalApplicationUrl is valid"),
            new TestCaseData("http://url",
                    "'External Application Url' must not be specified based on the ApplicationMethod chosen", true)
                .SetName("Then a populated ExternalApplicationUrl is invalid")
        };

        private static List<TestCaseData> ExternalApplicationInstructionsCases() => new List<TestCaseData>
        {
            new TestCaseData(null, null, false)
                .SetName("Then a null ExternalApplicationInstructions is valid"),
            new TestCaseData("", null, false)
                .SetName("Then an empty ExternalApplicationInstructions is valid"),
            new TestCaseData("instruction",
                    "'External Application Instructions' must not be specified based on the ApplicationMethod chosen", true)
                .SetName("Then a populated ExternalApplicationInstructions is invalid")
        };

        [TestCaseSource(nameof(Question1TestCases))]
        public void ValidateSupplementaryQuestion1(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                ApplicationMethod = ApplicationMethod.Online,
                SupplementaryQuestion1 = value
            };

            var sut = new Fixture().Customize(new AutoMoqCustomization())
                                   .Create<CreateApprenticeshipRequestValidator>();

            sut.Validate(request);

            if (shouldError)
            {
                sut.ShouldHaveValidationErrorFor(req => req.SupplementaryQuestion1, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion1)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.SupplementaryQuestion1, request);
            }
        }

        [TestCaseSource(nameof(Question2TestCases))]
        public void ValidateSupplementaryQuestion2(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                ApplicationMethod = ApplicationMethod.Online,
                SupplementaryQuestion2 = value
            };

            var sut = new Fixture().Customize(new AutoMoqCustomization())
                                   .Create<CreateApprenticeshipRequestValidator>();

            sut.Validate(request);

            if (shouldError)
            {
                sut.ShouldHaveValidationErrorFor(req => req.SupplementaryQuestion2, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.SupplementaryQuestion2)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.SupplementaryQuestion2, request);
            }
        }

        [TestCaseSource(nameof(ExternalApplicationUrlCases))]
        public void ValidateExternalApplicationUrl(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                ApplicationMethod = ApplicationMethod.Online,
                ExternalApplicationUrl = value
            };

            var sut = new Fixture().Customize(new AutoMoqCustomization())
                                   .Create<CreateApprenticeshipRequestValidator>();

            sut.Validate(request);

            if (shouldError)
            {
                sut.ShouldHaveValidationErrorFor(req => req.ExternalApplicationUrl, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.ExternalApplicationUrl)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.ExternalApplicationUrl, request);
            }
        }

        [TestCaseSource(nameof(ExternalApplicationInstructionsCases))]
        public void ValidateExternalApplicationInstructions(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                ApplicationMethod = ApplicationMethod.Online,
                ExternalApplicationInstructions = value
            };

            var sut = new Fixture().Customize(new AutoMoqCustomization())
                                   .Create<CreateApprenticeshipRequestValidator>();

            sut.Validate(request);

            if (shouldError)
            {
                sut.ShouldHaveValidationErrorFor(req => req.ExternalApplicationInstructions, request)
                   .WithErrorCode(ErrorCodes.CreateApprenticeship.ExternalApplicationInstructions)
                   .WithErrorMessage(errorMessage);
            }
            else
            {
                sut.ShouldNotHaveValidationErrorFor(req => req.ExternalApplicationInstructions, request);
            }
        }
    }
}
