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
    public class AndApplicationMethodIsOffline
    {
        private static List<TestCaseData> ExternalApplicationUrlCases() => new List<TestCaseData>
        {
            new TestCaseData(null, "'External Application Url' should not be empty.", true)
                .SetName("Then a null ExternalApplicationUrl is invalid"),
            new TestCaseData("", "'External Application Url' should not be empty.", true)
                .SetName("Then an empty ExternalApplicationUrl is invalid"),
            new TestCaseData("testdomain", "'External Application Url' must be a valid Url", true)
                .SetName("Then an incomplete Url should be invalid"),
            new TestCaseData("ftp://testdomain", "'External Application Url' must be a valid Url", true)
                .SetName("Then a non http(s) Url should be invalid"),
            new TestCaseData("http://testdomain.com", null, false)
                .SetName("Then a complete Url should be valid")
        };

        private static List<TestCaseData> ExternalApplicationInstructionsCases() => new List<TestCaseData>
        {
            new TestCaseData(null, null, false)
                .SetName("Then a null ExternalApplicationInstructions is valid"),
            new TestCaseData("", null, false)
                .SetName("Then an empty ExternalApplicationInstructions is valid"),
            new TestCaseData(new String('b', 4000), null, false)
                .SetName("Then an instruction up to 4000 characters is valid"),
            new TestCaseData(new String('a', 4001),
                    "'External Application Instructions' must be less than 4001 characters. You entered 4001 characters.", true)
                .SetName("Then an ExternalApplicationInstructions greater than 4000 characters is invalid"),
            new TestCaseData("<", "'External Application Instructions' can't contain invalid characters", true)
                .SetName("Then ExternalApplicationInstructions should not contain invalid characters")
        };

        private static List<TestCaseData> SupplementaryQuestion1TestCases() => new List<TestCaseData>
        {
            new TestCaseData(null, null, false)
                .SetName("Then a null SupplementaryQuestion1 is valid"),
            new TestCaseData("", null, false)
                .SetName("Then an empty SupplementaryQuestion1 is valid"),
            new TestCaseData("instruction",
                    "You cannot specify Supplementary Questions for this Application Method", true)
                .SetName("Then a populated SupplementaryQuestion1 is invalid")
        };

        private static List<TestCaseData> SupplementaryQuestion2TestCases() => new List<TestCaseData>
        {
            new TestCaseData(null, null, false)
                .SetName("Then a null SupplementaryQuestion2 is valid"),
            new TestCaseData("", null, false)
                .SetName("Then an empty SupplementaryQuestion2 is valid"),
            new TestCaseData("instruction",
                    "You cannot specify Supplementary Questions for this Application Method", true)
                .SetName("Then a populated SupplementaryQuestion2 is invalid")
        };

        [TestCaseSource(nameof(ExternalApplicationUrlCases))]
        public void ValidateExternalApplicationUrl(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                ApplicationMethod = ApplicationMethod.Offline,
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
                ApplicationMethod = ApplicationMethod.Offline,
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

        [TestCaseSource(nameof(SupplementaryQuestion1TestCases))]
        public void ValidateSupplementaryQuestion1(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                ApplicationMethod = ApplicationMethod.Offline,
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

        [TestCaseSource(nameof(SupplementaryQuestion2TestCases))]
        public void ValidateSupplementaryQuestion2(string value, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                ApplicationMethod = ApplicationMethod.Offline,
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
    }
}
