using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipCommandHandler
{
    [TestFixture]
    public class WhenGetTrainingLevelAsync
    {
        [Test]
        public void AndTrainingCodeIsNotFound_ThenRaiseValidationException()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            fixture.Freeze<Mock<IValidator<CreateApprenticeshipRequest>>>(composer =>
                composer.Do(mock => mock
                    .Setup(validator => validator.ValidateAsync(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new ValidationResult())));

            fixture.Freeze<Mock<ITrainingDetailService>>(composer =>
                composer.Do(mock => mock
                    .Setup(svc =>
                        svc.GetAllFrameworkDetailsAsync()).ReturnsAsync(new List<TrainingDetail>())));

            var handler = fixture.Create<CreateApprenticeshipCommandHandler>();

            var request = fixture.Create<CreateApprenticeshipRequest>();

            var action = new Func<Task<CreateApprenticeshipResponse>>(() => handler.Handle(request));

            const string errorMessage = "'Training Code' is invalid.";
            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {errorMessage}");

        }

        [Test]
        public void AndTrainingCodeHasExpired_ThenRaiseValidationException()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            fixture.Freeze<Mock<IValidator<CreateApprenticeshipRequest>>>(composer =>
                composer.Do(mock => mock
                                    .Setup(validator => validator.ValidateAsync(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(new ValidationResult())));

            fixture.Freeze<Mock<ITrainingDetailService>>(composer =>
                composer.Do(mock => mock
                                    .Setup(svc => svc.GetAllFrameworkDetailsAsync())
                                    .ReturnsAsync(new List<TrainingDetail>()
                                    {
                                        new TrainingDetail { TrainingCode = "405-3-1", EffectiveTo = DateTime.Today.AddDays(-1) }
                                    })));

            var handler = fixture.Create<CreateApprenticeshipCommandHandler>();

            var request = fixture.Create<CreateApprenticeshipRequest>();
            request.TrainingCode = "405-3-1";

            var action = new Func<Task<CreateApprenticeshipResponse>>(() => handler.Handle(request));

            const string errorMessage = "'Training Code' is invalid.";
            action.ShouldThrow<ValidationException>()
                  .WithMessage($"Validation failed: \r\n -- {errorMessage}");
        }

        [Test]
        public async Task AndTrainingCodeHasNotExpired_ThenRaiseValidationExceptionAsync()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            fixture.Freeze<Mock<IValidator<CreateApprenticeshipRequest>>>(composer =>
                composer.Do(mock => mock
                                    .Setup(validator => validator.ValidateAsync(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(new ValidationResult())));

            fixture.Freeze<Mock<ITrainingDetailService>>(composer =>
                composer.Do(mock => mock
                                    .Setup(svc => svc.GetAllFrameworkDetailsAsync())
                                    .ReturnsAsync(new List<TrainingDetail>()
                                    {
                                        new TrainingDetail { TrainingCode = "405-3-1", EffectiveTo = DateTime.Today.AddDays(1) }
                                    })));

            fixture.Freeze<Mock<IVacancyOwnerService>>(composer => composer.Do(mock =>
                mock.Setup(svc => svc.GetEmployersInformationAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(new EmployerInformation())));

            var request = fixture.Create<CreateApprenticeshipRequest>();
            request.TrainingCode = "405-3-1";

            var handler = fixture.Create<CreateApprenticeshipCommandHandler>();
            var response = await handler.Handle(request).ConfigureAwait(false);

            response.Should().BeOfType<CreateApprenticeshipResponse>();
        }
    }
}