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
    public class WhenValidatingWorkingWeek : CreateApprenticeshipRequestValidatorBase
    {
        private const string TextLengthOver250 = "2l;kmp0fo 98sadiljrfqp23 9 8jpqao we98pjasod ifjpqwaoerjfa psoidf jasp o e i djq wp oei fm asp odijfpaoiw ertoi pqweht98ww a oiwerjhfao ooo ookm p0 fo 98s adi ljrfqp2398jpqaowe98 pjas odi fjp qwa oe rjf apsoidfja spoeidjqw poeifmaspod ijfpao iwertoipx";
        private const string TextLengthAt250 = "2l;kmp0fo 98sadiljrfqp23 9 8jpqao we98pjasod ifjpqwaoerjfa psoidf jasp o e i djq wp oei fm asp odijfpaoiw ertoi pqweht98ww a oiwerjhfao ooo ookm p0 fo 98s adi ljrfqp2398jpqaowe98 pjas odi fjp qwa oe rjf apsoidfja spoeidjqw poeifmaspod ijfpao iwertoix";
        private const string TextLengthUnder250 = "2l;kmp0fo 98sadiljrfqp23 9 8jpqao we98pjasod ifjpqwaoerjfa psoidf jasp o e i djq wp oei fm asp odijfpaoiw ertoi pqweht98ww a oiwerjhfao ooo ookm p0 fo 98s adi ljrfqp2398jpqaowe98 pjas odi fjp qwa oe rjf apsoidfja spoeidjqw poeifmaspod ijfpao iwertsx";
        private const string TextWithInvalidChars = "<hax0rs_unite/>2l;kmp0fo 98sadiljrfqp23 9 8jpqao we98pjasod ifjpqwaoerjfa psoidf jasp o e i djq wp oei fm asp odijfpaoiw ertoi pqweht98ww a oiwerjhfao ooo ookm p0 fo 98s adi ljrfqp2398jpqaowe98 pjas odi fjp qwa oe rjf ";

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(null, false,
                ErrorCodes.CreateApprenticeship.WorkingWeekRequired,
                "'Working Week' should not be empty.")
            .SetName("And is null Then is invalid"),
            new TestCaseData(TextLengthOver250, false,
                ErrorCodes.CreateApprenticeship.WorkingWeekLengthGreaterThan250,
                "'Working Week' must be less than 251 characters. You entered 251 characters.")
            .SetName("And is over 250 chars length Then is invalid"),
            new TestCaseData(TextLengthAt250, true, null, null)
            .SetName("And is 250 chars length Then is valid"),
            new TestCaseData(TextLengthUnder250, true, null, null)
            .SetName("And is under 250 chars length Then is valid"),
            new TestCaseData(TextWithInvalidChars, false,
                ErrorCodes.CreateApprenticeship.WorkingWeekShouldNotIncludeSpecialCharacters,
                "'Working Week' can't contain invalid characters")
            .SetName("And has restricted characters Then is invalid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCallingValidate(string stringToValidate, bool expectedIsValid,
            string expectedErrorCodes, string expectedErrorMessages)
        {
            var fixture = new Fixture();

            var request = new CreateApprenticeshipRequest
            {
                WorkingWeek = stringToValidate
            };

            var context = GetValidationContextForProperty(request, req => req.WorkingWeek);

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