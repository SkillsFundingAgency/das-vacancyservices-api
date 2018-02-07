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
    public class WhenValidatingWageTypeReason : CreateApprenticeshipRequestValidatorBase
    {
        private static readonly string WageTypeReasonLength240 = new string('a', 240);
        private static readonly string WageTypeReasonLength241 = new string('a', 241);
        private static readonly string WageTypeReasonHtml = "<p>I like <strong onmouseover='prompt(\"you've been hax0rd\");'>html</strong></p>";

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(null, true, null, null)
                .SetName("And is null Then is valid"),
            new TestCaseData("", true, null, null)
                .SetName("And is empty string Then is valid"),
            new TestCaseData(WageTypeReasonLength241, false,
                    ErrorCodes.CreateApprenticeship.WageTypeReason,
                    $"'Wage Type Reason' must be less than 241 characters. You entered {WageTypeReasonLength241.Length} characters.")
                .SetName("And has length 241 Then is invalid"),
            new TestCaseData(WageTypeReasonHtml, false,
                    ErrorCodes.CreateApprenticeship.WageTypeReason,
                    "'Wage Type Reason' can't contain invalid characters")
                .SetName("And has html Then is invalid"),
            new TestCaseData(WageTypeReasonLength240, true, null, null)
                .SetName("And has length 240 Then is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCallingValidate(string wageTypeReasonToValidate, bool expectedIsValid,
            string expectedErrorCodes, string expectedErrorMessages)
        {
            var fixture = new Fixture();
            var request = new CreateApprenticeshipRequest
            {
                WageTypeReason = wageTypeReasonToValidate
            };

            var context = GetValidationContextForProperty(request, req => req.WageTypeReason);

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