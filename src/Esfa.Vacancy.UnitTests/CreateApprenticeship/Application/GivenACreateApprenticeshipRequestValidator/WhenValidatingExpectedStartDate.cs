using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingExpectedStartDate : CreateApprenticeshipRequestValidatorBase
    {
        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(null, DateTime.Today.AddDays(1), false,
                    new List<string> { ErrorCodes.CreateApprenticeship.ExpectedStartDateRequired },
                    new List<string> { "'Expected Start Date' should not be empty." })
                .SetName("And is null Then is invalid"),
            new TestCaseData(DateTime.Today.AddDays(3), DateTime.Today.AddDays(4), false,
                    new List<string> { ErrorCodes.CreateApprenticeship.ExpectedStartDateBeforeClosingDate },
                    new List<string> { "'Expected Start Date' must be after the specified closing date." })
                .SetName("And is before closing date Then is invalid"),
            new TestCaseData(DateTime.Today.AddDays(3), DateTime.Today.AddDays(3), false,
                    new List<string> { ErrorCodes.CreateApprenticeship.ExpectedStartDateBeforeClosingDate },
                    new List<string> { "'Expected Start Date' must be after the specified closing date." })
                .SetName("And is on closing date Then is invalid"),
            new TestCaseData(DateTime.Today.AddDays(3).AddHours(12), DateTime.Today.AddDays(3), false,
                    new List<string> { ErrorCodes.CreateApprenticeship.ExpectedStartDateBeforeClosingDate },
                    new List<string> { "'Expected Start Date' must be after the specified closing date." })
                .SetName("And is on closing date (with hrs) Then is invalid"),
            new TestCaseData(DateTime.Today.AddDays(3), DateTime.Today.AddDays(2), true,
                    new List<string>(),
                    new List<string>())
                .SetName("And is after closing date Then is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCallingValidate(DateTime dateToValidate, DateTime closingDate, bool expectedIsValid,
            List<string> expectedErrorCodes, List<string> expectedErrorMessages)
        {
            var fixture = new Fixture();

            var request = fixture.Build<CreateApprenticeshipRequest>()
                .With(req => req.ExpectedStartDate, dateToValidate)
                .With(req => req.ApplicationClosingDate, closingDate)
                .Create();

            var context = GetValidationContextForProperty(request, req => req.ExpectedStartDate);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(expectedIsValid);
            result.Errors.Select(failure => failure.ErrorCode)
                .ShouldAllBeEquivalentTo(expectedErrorCodes);
            result.Errors.Select(failure => failure.ErrorMessage)
                .ShouldAllBeEquivalentTo(expectedErrorMessages);
        }
    }
}