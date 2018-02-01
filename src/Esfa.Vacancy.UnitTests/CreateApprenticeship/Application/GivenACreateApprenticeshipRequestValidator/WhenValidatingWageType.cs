using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingWageType: CreateApprenticeshipRequestValidatorBase
    {
        private const int InvalidWageType = 234;
        private const WageType ValidWageType = WageType.ApprenticeshipMinimumWage;

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(null, false,
                ErrorCodes.CreateApprenticeship.WageTypeError,
                "'Wage Type' should not be empty.")
            .SetName("And is null Then is invalid"),
            new TestCaseData((WageType)InvalidWageType, false,
                ErrorCodes.CreateApprenticeship.WageTypeError,
                $"'Wage Type' has a range of values which does not include '{InvalidWageType}'.")
            .SetName("And is not a known wage type Then is invalid"),
            new TestCaseData(ValidWageType, true, null, null)
            .SetName("And is a known wage type Then is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCallingValidate(WageType wageTypeToValidate, bool expectedIsValid,
            string expectedErrorCodes, string expectedErrorMessages)
        {
            var fixture = new Fixture();
            var request = new CreateApprenticeshipRequest
            {
                WageType = wageTypeToValidate
            };

            var context = GetValidationContextForProperty(request, req => req.WageType);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(expectedIsValid);
            if (!expectedIsValid)
            {
                result.Errors.First().ErrorCode
                    .Should().Be(expectedErrorCodes);
                result.Errors.First().ErrorMessage
                    .Should().Be(expectedErrorMessages);
            }
        }
    }
}