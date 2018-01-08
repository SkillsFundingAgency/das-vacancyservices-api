using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
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
            new TestCaseData(DateTime.Today, false).SetName("And is today Then is invalid"),
            new TestCaseData(DateTime.Today.AddDays(1), true).SetName("And is tomorrow Then is valid"),
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCallingValidate(DateTime dateToValidate, bool expectedIsValid)
        {
            var fixture = new Fixture();

            var request = fixture.Build<CreateApprenticeshipRequest>()
                .With(req => req.ApplicationClosingDate, dateToValidate)
                .Create();

            var context = GetValidationContextForProperty(request, req => req.ApplicationClosingDate);

             var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(expectedIsValid);
        }
    }
}