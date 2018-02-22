using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.WhenValidatingTrainingCode
{
    [TestFixture]
    public class AndTrainingTypeIsFramework
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
            "123",
            "'Training Code' should be in the format '###-##-##' when Training Type is Framework.",
            true,
            TestName = "And is not in expected format then is invalid.")]
        [TestCase(
            "123-12-1",
            null,
            false,
            TestName = "And is in expected format then is valid.")]
        public void ValidateTrainingCode(string trainingCode, string errorMessage, bool shouldError)
        {
            var request = new CreateApprenticeshipRequest
            {
                TrainingType = TrainingType.Framework,
                TrainingCode = trainingCode
            };

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            if (shouldError)
            {
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