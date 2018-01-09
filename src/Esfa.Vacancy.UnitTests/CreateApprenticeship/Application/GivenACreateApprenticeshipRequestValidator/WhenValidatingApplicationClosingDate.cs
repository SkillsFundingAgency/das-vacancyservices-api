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
    public class WhenValidatingApplicationClosingDate : CreateApprenticeshipRequestValidatorBase
    {
        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(null, false, 
                new List<string> { ErrorCodes.CreateApprenticeship.ApplicationClosingDateRequired }, 
                new List<string> { "'Application Closing Date' should not be empty." })
            .SetName("And is null Then is invalid"),
            new TestCaseData(DateTime.Today, false, 
                new List<string> { ErrorCodes.CreateApprenticeship.ApplicationClosingDateBeforeTomorrow }, 
                new List<string> { ErrorMessages.CreateApprenticeship.ApplicationClosingDateBeforeTomorrow })
            .SetName("And is today Then is invalid"),
            new TestCaseData(DateTime.Today.AddHours(12), false,
                new List<string> { ErrorCodes.CreateApprenticeship.ApplicationClosingDateBeforeTomorrow },
                new List<string> { ErrorMessages.CreateApprenticeship.ApplicationClosingDateBeforeTomorrow })
            .SetName("And is today (with hrs) Then is invalid"),
            new TestCaseData(DateTime.Today.AddDays(1), true, 
                new List<string>(), 
                new List<string>())
            .SetName("And is tomorrow Then is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCallingValidate(DateTime dateToValidate, bool expectedIsValid, 
            List<string> expectedErrorCodes, List<string> expectedErrorMessages)
        {
            var fixture = new Fixture();

            var request = fixture.Build<CreateApprenticeshipRequest>()
                .With(req => req.ApplicationClosingDate, dateToValidate)
                .Create();

            var context = GetValidationContextForProperty(request, req => req.ApplicationClosingDate);

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