using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Repositories;
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
        private CreateApprenticeshipResponse _createApprenticeshipResponse;
        private int _expectedRefNumber;
        private Mock<IValidator<CreateApprenticeshipRequest>> _mockValidator;
        private IFixture _fixture;
        private CreateApprenticeshipCommandHandler _handler;
        private Mock<ICreateApprenticeshipParametersMapper> _mockMapper;
        private Mock<IVacancyRepository> _mockRepository;
        private CreateApprenticeshipRequest _validRequest;
        private CreateApprenticeshipParameters _expectedParameters;

        [SetUp]
        public async Task SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _expectedRefNumber = _fixture.Create<int>();
            _expectedParameters = _fixture.Freeze<CreateApprenticeshipParameters>();
            _validRequest = _fixture.Create<CreateApprenticeshipRequest>();

            _mockValidator = _fixture.Freeze<Mock<IValidator<CreateApprenticeshipRequest>>>();
            _mockValidator
                .Setup(validator => validator.ValidateAsync(_validRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockMapper = _fixture.Freeze<Mock<ICreateApprenticeshipParametersMapper>>(composer => composer.Do(mock => mock
                .Setup(mapper => mapper.MapFromRequest(It.IsAny<CreateApprenticeshipRequest>()))
                .Returns(_expectedParameters)));

            _mockRepository = _fixture.Freeze<Mock<IVacancyRepository>>(composer => composer.Do(mock => mock
                .Setup(repository => repository.CreateApprenticeshipAsync(It.IsAny<CreateApprenticeshipParameters>()))
                .ReturnsAsync(_expectedRefNumber)));

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
        public void ThenValidatesRequest()
        {
            _mockValidator.Verify(validator => 
                validator.ValidateAsync(_validRequest, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public void ThenMapsRequestToCreateParams()
        {
            _mockMapper.Verify(mapper => 
                mapper.MapFromRequest(_validRequest),
                Times.Once);
        }

        [Test]
        public void ThenCreatesVacancy()
        {
            _mockRepository.Verify(repository => 
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