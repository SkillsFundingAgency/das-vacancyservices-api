using System.Linq;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingEmployerEdsUrn
    {
        [TestCase(0, false, "'Employer Eds Urn' should not be empty.", TestName = "And is zero Then is invalid")]
        [TestCase(-1, false, "'Employer Eds Urn' must be greater than '0'.", TestName = "And is less than zero Then is invalid")]
        [TestCase(1, true, "", TestName = "And is greater than zero Then is valid")]
        public void ValidateEmployerEdsUrn(int value, bool shouldBeValid, string errorMessage)
        {
            var request = new CreateApprenticeshipRequest()
            {
                EmployerEdsUrn = value
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (shouldBeValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.EmployerEdsUrn, request);
            }
            else
            {
                var result = validator.ShouldHaveValidationErrorFor(r => r.EmployerEdsUrn, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.EmployerEdsUrn)
                    .WithErrorMessage(errorMessage);
                result.Count().Should().Be(1);
            }
        }
    }
}