using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingTrainingType
    {
        [TestCase(0, "'Training Type' has a range of values which does not include '0'.", true, TestName = "And if 0 then is invalid.")]
        [TestCase(44, "'Training Type' has a range of values which does not include '44'.", true, TestName = "And if unexpected value then is invalid.")]
        [TestCase(TrainingType.Framework, null, false, TestName = "And if Framework then is valid.")]
        [TestCase(TrainingType.Standard, null, false, TestName = "And if Standard then is valid.")]
        public void ValidateTrainingType(TrainingType trainingType, string errorMessage, bool shouldError)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var request = fixture.Build<CreateApprenticeshipRequest>()
                .With(req => req.TrainingType, trainingType)
                .Create();

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            if (shouldError)
            {
                validator
                    .ShouldHaveValidationErrorFor(req => req.TrainingType, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingType)
                    .WithErrorMessage(errorMessage);
            }
            else
            {
                validator.ShouldNotHaveValidationErrorFor(req => req.TrainingType, request);
            }
        }
    }
}