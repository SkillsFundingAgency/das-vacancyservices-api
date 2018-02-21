using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Domain.Validation;
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
    public class WhenHandlingACommand
    {
        private EmployerInformation _employerInformation;
        private CreateApprenticeshipResponse _createApprenticeshipResponse;
        private int _expectedRefNumber;
        private Mock<IValidator<CreateApprenticeshipRequest>> _mockValidator;
        private IFixture _fixture;
        private CreateApprenticeshipCommandHandler _handler;
        private Mock<ICreateApprenticeshipParametersMapper> _mockMapper;
        private Mock<ICreateApprenticeshipService> _mockService;
        private CreateApprenticeshipRequest _validRequest;
        private CreateApprenticeshipParameters _expectedParameters;
        private Mock<IVacancyOwnerService> _mockVacancyOwnerService;
        private Mock<ITrainingDetailService> _mockTrainingDetailService;
        private TrainingDetail _trainingDetail;


        [SetUp]
        public async Task SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _employerInformation = _fixture.Create<EmployerInformation>();
            _expectedRefNumber = _fixture.Create<int>();
            _expectedParameters = _fixture.Freeze<CreateApprenticeshipParameters>();
            _validRequest = _fixture.Create<CreateApprenticeshipRequest>();

            _mockValidator = _fixture.Freeze<Mock<IValidator<CreateApprenticeshipRequest>>>(composer =>
                composer.Do(mock => mock
                    .Setup(validator => validator.ValidateAsync(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new ValidationResult())));

            _mockVacancyOwnerService = _fixture.Freeze<Mock<IVacancyOwnerService>>(composer =>
                composer.Do(mock => mock
                    .Setup(svc => svc.GetEmployersInformationAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(_employerInformation)));

            _mockMapper = _fixture.Freeze<Mock<ICreateApprenticeshipParametersMapper>>(composer =>
                composer.Do(mock => mock
                    .Setup(mapper => mapper.MapFromRequest(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<EmployerInformation>()))
                    .Returns(_expectedParameters)));

            _mockService = _fixture.Freeze<Mock<ICreateApprenticeshipService>>(composer =>
                composer.Do(mock => mock
                    .Setup(repository => repository.CreateApprenticeshipAsync(It.IsAny<CreateApprenticeshipParameters>()))
                    .ReturnsAsync(_expectedRefNumber)));

            _trainingDetail = _fixture.Create<TrainingDetail>();
            _mockTrainingDetailService = _fixture.Freeze<Mock<ITrainingDetailService>>(composer =>
                composer.Do(mock =>
                {
                    mock
                        .Setup(svc => svc.GetFrameworkDetailsAsync(It.IsAny<string>()))
                        .ReturnsAsync(_trainingDetail);
                    mock
                        .Setup(svc => svc.GetStandardDetailsAsync(It.IsAny<string>()))
                        .ReturnsAsync(_trainingDetail);
                }));

            _handler = _fixture.Create<CreateApprenticeshipCommandHandler>();

            _createApprenticeshipResponse = await _handler.Handle(_validRequest);
        }

        [Test]
        public void AndCommandNotValid_ThenThrowsValidationException()
        {
            var errorMessage = _fixture.Create<string>();
            var invalidRequest = _fixture.Create<CreateApprenticeshipRequest>();

            _mockValidator
                .Setup(validator => validator.ValidateAsync(invalidRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("stuff", errorMessage)
                }));

            var action = new Func<Task<CreateApprenticeshipResponse>>(() => _handler.Handle(invalidRequest));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {errorMessage}");
        }

        [Test]
        public async Task AndFrameworkCodeIsNotFound_ThenSetIsValidToFalse()
        {
            var request = _fixture.Build<CreateApprenticeshipRequest>()
                .With(r => r.TrainingType, TrainingType.Framework)
                .With(r => r.TrainingEffectiveTo, null)
                .With(r => r.EducationLevel, 0)
                .Create();

            _mockTrainingDetailService.Setup(svc => svc.GetFrameworkDetailsAsync(It.IsAny<string>())).ReturnsAsync((TrainingDetail)null);

            await _handler.Handle(request);

            _mockTrainingDetailService.Verify(svc => svc.GetFrameworkDetailsAsync(It.IsAny<string>()));

            request.IsTrainingCodeValid.Should().BeFalse();
            request.TrainingEffectiveTo.Should().BeNull();
            request.EducationLevel.Should().Be(0);
        }

        [Test]
        public async Task AndTrainingTypeIsFramework_ThenUpdateTrainingCode()
        {
            var request = _fixture.Create<CreateApprenticeshipRequest>();
            request.TrainingType = TrainingType.Framework;

            await _handler.Handle(request);

            _mockTrainingDetailService.Verify(svc => svc.GetFrameworkDetailsAsync(It.IsAny<string>()));
            request.TrainingEffectiveTo.Should().Be(_trainingDetail.EffectiveTo);
            request.IsTrainingCodeValid.Should().BeTrue();
            request.EducationLevel.Should().Be(_trainingDetail.Level);
        }

        [Test]
        public async Task AndTrainingTypeIsStandard_ThenUpdateTrainingCode()
        {
            var request = _fixture.Build<CreateApprenticeshipRequest>()
                .With(r => r.TrainingEffectiveTo, null)
                .With(r => r.TrainingType, TrainingType.Standard)
                .Create();

            await _handler.Handle(request);

            _mockTrainingDetailService.Verify(svc => svc.GetStandardDetailsAsync(It.IsAny<string>()));
            request.TrainingEffectiveTo.Should().Be(_trainingDetail.EffectiveTo);
            request.IsTrainingCodeValid.Should().BeTrue();
            request.EducationLevel.Should().Be(_trainingDetail.Level);
        }

        [Test]
        public async Task AndStandardCodeIsNotFound_ThenSetIsValidToFalse()
        {
            var request = _fixture.Build<CreateApprenticeshipRequest>()
                .With(r => r.TrainingEffectiveTo, null)
                .With(r => r.TrainingType, TrainingType.Standard)
                .With(r => r.TrainingEffectiveTo, null)
                .With(r => r.EducationLevel, 0)
                .Create();

            _mockTrainingDetailService.Setup(svc => svc.GetStandardDetailsAsync(It.IsAny<string>())).ReturnsAsync((TrainingDetail)null);

            await _handler.Handle(request);

            _mockTrainingDetailService.Verify(svc => svc.GetStandardDetailsAsync(It.IsAny<string>()));

            request.IsTrainingCodeValid.Should().BeFalse();
            request.TrainingEffectiveTo.Should().BeNull();
            request.EducationLevel.Should().Be(0);
        }

        [Test]
        public void ThenValidatesRequest()
        {
            _mockValidator.Verify(validator =>
                validator.ValidateAsync(_validRequest, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public void ThenFetchVacancyOwnerLink()
        {
            _mockVacancyOwnerService.Verify(svc => svc.GetEmployersInformationAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
        }

        [Test]
        public void AndIfLinkIsNotRetrieved_ThenThrowUnauthorisedException()
        {
            _mockVacancyOwnerService
                .Setup(svc => svc.GetEmployersInformationAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((EmployerInformation)null);

            var action = new Func<Task<CreateApprenticeshipResponse>>(() => _handler.Handle(_validRequest));
            action.ShouldThrow<UnauthorisedException>()
                .WithMessage(ErrorMessages.CreateApprenticeship.MissingProviderSiteEmployerLink);
        }

        [Test]
        public void ThenMapsRequestToCreateParams()
        {
            _mockMapper.Verify(mapper =>
                mapper.MapFromRequest(_validRequest, _employerInformation),
                Times.Once);
        }

        [Test]
        public void ThenCreatesVacancy()
        {
            _mockService.Verify(repository =>
                repository.CreateApprenticeshipAsync(_expectedParameters),
                Times.Once);
        }

        [Test]
        public void ThenReturnsRefNumberFromRepository()
        {
            _createApprenticeshipResponse.VacancyReferenceNumber
                .Should().Be(_expectedRefNumber);
        }
    }
}