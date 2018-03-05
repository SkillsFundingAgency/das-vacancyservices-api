using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.WhenValidatingLocation.AndLocationTypeOfOther
{
    [TestFixture]
    public class AndValidatingAdditionalLocationInformation
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, true, null)
                    .SetName("And is null Then is valid"),
                new TestCaseData("null", true, null)
                    .SetName("And is in allowed format Then is valid"),
                new TestCaseData(new string('a', 4001), false, "'Additional Location Information' must be less than 4001 characters. You entered 4001 characters.")
                    .SetName("And exceeds 4000 characters Then is invalid"),
                new TestCaseData("<p>", false, "'Additional Location Information' can't contain invalid characters")
                    .SetName("And contains illegal chars Then is invalid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAdditionalLocationInformation(
            string additionalLocationInformation, bool isValid, string errorMessage)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = LocationType.OtherLocation,
                AdditionalLocationInformation = additionalLocationInformation
            };

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

            if (isValid)
            {
                sut.ShouldNotHaveValidationErrorFor(r => r.AdditionalLocationInformation, request);
            }
            else
            {
                sut
                    .ShouldHaveValidationErrorFor(r => r.AdditionalLocationInformation, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AdditionalLocationInformation)
                    .WithErrorMessage(errorMessage);
            }
        }
    }
}