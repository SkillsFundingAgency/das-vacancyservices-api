using System.Linq;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingNumberOfPositions
    {
        [TestCase(0, false, "'Number Of Positions' should not be empty.",
            TestName = "And is zero then it is invalid.")]
        [TestCase(5000, true, null,
            TestName = "And is less than or equal to 5000 then is valid")]
        [TestCase(5001, false, "'Number Of Positions' must be between 1 and 5000. You entered 5001.",
            TestName = "And is greater than 5000 then is invalid")]
        [TestCase(-1, false, "'Number Of Positions' must be between 1 and 5000. You entered -1.",
            TestName = "And is less than zero then is invalid")]
        public void ValidateNumberOfPositions(int numberOfPositions, bool isValid, string errorMessage)
        {
            var request = new CreateApprenticeshipRequest()
            {
                NumberOfPositions = numberOfPositions
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (isValid)
            {
                validator.ShouldNotHaveValidationErrorFor(x => x.NumberOfPositions, request);
            }
            else
            {
                var errors = validator
                    .ShouldHaveValidationErrorFor(x => x.NumberOfPositions, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.NumberOfPositions)
                    .WithErrorMessage(errorMessage);
                errors.Count().Should().Be(1);
            }
        }
    }
}