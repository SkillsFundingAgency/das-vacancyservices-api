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
    public class WhenValidatingHoursPerWeek : CreateApprenticeshipRequestValidatorBase
    {
        private const double HoursLessThan16 = 15.999999;
        private const double HoursEquals16 = 16;
        private const double HoursBetween16And48 = 25.3245;
        private const double HoursEquals48 = 48;
        private const double HoursGreaterThan48 = 48.0000001;

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(null, false,
                ErrorCodes.CreateApprenticeship.HoursPerWeek,
                "'Hours Per Week' should not be empty.")
            .SetName("And is null Then is invalid"),
            new TestCaseData(HoursLessThan16, false,
                ErrorCodes.CreateApprenticeship.HoursPerWeek,
                $"'Hours Per Week' must be between 16 and 48. You entered {HoursLessThan16}.")
            .SetName("And is under 16 Then is invalid"),
            new TestCaseData(HoursEquals16, true, null, null)
            .SetName("And is 16 Then is valid"),
            new TestCaseData(HoursBetween16And48, true, null, null)
            .SetName("And is between 16 and 48 Then is valid"),
            new TestCaseData(HoursEquals48, true, null, null)
                .SetName("And is 48 Then is valid"),
            new TestCaseData(HoursGreaterThan48, false,
                ErrorCodes.CreateApprenticeship.HoursPerWeek,
                $"'Hours Per Week' must be between 16 and 48. You entered {HoursGreaterThan48}.")
            .SetName("And is greater than 48 Then is invalid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCallingValidate(double hoursToValidate, bool expectedIsValid,
            string expectedErrorCodes, string expectedErrorMessages)
        {
            var fixture = new Fixture();

            var request = new CreateApprenticeshipRequest
            {
                HoursPerWeek = hoursToValidate
            };

            var context = GetValidationContextForProperty(request, req => req.HoursPerWeek);

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