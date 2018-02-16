using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.WhenValidatingTrainingCode
{
    [TestFixture]
    public class AndTrainingTypeIsStandard
    {
        [TestCase(
            null,
            "'Training Code' should not be empty.",
            true,
            TestName = "And is null then is invalid.")]
        [TestCase(
            "",
            "'Training Code' should not be empty.",
            true,
            TestName = "And is empty then is invalid.")]
        [TestCase(
            "dfad",
            "'Training Code' should be a number between 1 and 9999 when Training Type is Standard.",
            true,
            TestName = "And is not a number then is invalid.")]
        [TestCase(
            "10000",
            "'Training Code' should be a number between 1 and 9999 when Training Type is Standard.",
            true,
            TestName = "And is greater than 9999 then is invalid.")]
        [TestCase(
            "789",
            null,
            false,
            TestName = "And is less than 9999 then is valid.")]
        public void ValidateTrainingCode(string trainingCode, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                TrainingType = TrainingType.Standard,
                TrainingCode = trainingCode
            };

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            if (shouldError)
            {
                var s = validator.Validate(request);
                validator
                    .ShouldHaveValidationErrorFor(req => req.TrainingCode, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingCode)
                    .WithErrorMessage(errorMessage);
            }
            else
            {
                validator.ShouldNotHaveValidationErrorFor(req => req.TrainingCode, request);
            }
        }
    }
}