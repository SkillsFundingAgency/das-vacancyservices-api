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
    public class WhenValidatingIsTrainingCodeValid
    {
        [Test]
        public void WhenIsTrue_ThenIsValid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var request = fixture.Build<CreateApprenticeshipRequest>()
                .With(r => r.TrainingType, TrainingType.Standard)
                .With(r => r.TrainingCode, "123")
                .With(r => r.IsTrainingCodeValid, true)
                .Create();

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            validator
                .ShouldNotHaveValidationErrorFor(req => req.TrainingCode, request);
        }

        [Test]
        public void WhenIsFalse_ThenIsValid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var request = fixture.Build<CreateApprenticeshipRequest>()
                .With(r => r.TrainingType, TrainingType.Standard)
                .With(r => r.TrainingCode, "123")
                .With(r => r.IsTrainingCodeValid, false)
                .Create();

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            validator
                .ShouldHaveValidationErrorFor(req => req.IsTrainingCodeValid, request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingCode)
                .WithErrorMessage("'Training Code' is invalid.");
        }
    }
}