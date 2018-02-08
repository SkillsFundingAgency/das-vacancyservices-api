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
    public class WhenValidatingWageUnit : CreateApprenticeshipRequestValidatorBase
    {
        private const int InvalidWageUnit = 234;
        private const WageUnit ValidWageUnit = WageUnit.Annually;

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(null, false,
                ErrorCodes.CreateApprenticeship.WageUnit,
                "'Wage Unit' should not be empty.")
            .SetName("And is null Then is invalid"),
            new TestCaseData(0, false,
                    ErrorCodes.CreateApprenticeship.WageUnit,
                    "'Wage Unit' should not be empty.")
                .SetName("And is 0 Then is invalid"),
            new TestCaseData((WageUnit)InvalidWageUnit, false,
                ErrorCodes.CreateApprenticeship.WageUnit,
                $"'Wage Unit' has a range of values which does not include '{InvalidWageUnit}'.")
            .SetName("And is not a known wage unit Then is invalid"),
            new TestCaseData(ValidWageUnit, true, null, null)
            .SetName("And is a known wage unit Then is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCallingValidate(WageUnit wageUnitToValidate, bool expectedIsValid,
            string expectedErrorCodes, string expectedErrorMessages)
        {
            var fixture = new Fixture();
            var request = new CreateApprenticeshipRequest
            {
                WageUnit = wageUnitToValidate
            };

            var context = GetValidationContextForProperty(request, req => req.WageUnit);

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