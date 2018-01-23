using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
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
        [TestCase(5001, false, "'Number Of Positions' must be less than or equal to '5000'.",
            TestName = "And is greater than 5000 then is invalid")]
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
                const string errorCode = "31201";
                var s = validator.Validate(request);
                validator
                    .ShouldHaveValidationErrorFor(x => x.NumberOfPositions, request)
                    .WithErrorCode(errorCode)
                    .WithErrorMessage(errorMessage);
            }
        }
    }
}