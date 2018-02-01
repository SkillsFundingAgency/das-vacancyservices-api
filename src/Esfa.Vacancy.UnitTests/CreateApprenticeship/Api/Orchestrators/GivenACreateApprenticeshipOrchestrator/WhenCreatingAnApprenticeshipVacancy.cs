using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Api.Core;
using Esfa.Vacancy.Api.Core.Validation;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Manage.Api.Mappings;
using Esfa.Vacancy.Manage.Api.Orchestrators;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using ApiTypes = Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Api.Orchestrators.GivenACreateApprenticeshipOrchestrator
{
    [TestFixture]
    public class WhenCreatingAnApprenticeshipVacancy
    {
        private const int providerUkprn = 12345678;
        private Mock<IMediator> _mockMediator;
        private ApiTypes.CreateApprenticeshipResponse _actualResponse;
        private CreateApprenticeshipResponse _expectedMediatorResponse;
        private ApiTypes.CreateApprenticeshipResponse _expectedMapperResponse;
        private Mock<ICreateApprenticeshipResponseMapper> _mockResponseMapper;
        private Mock<ICreateApprenticeshipRequestMapper> _mockRequestMapper;
        private ApiTypes.CreateApprenticeshipParameters _actualParameters;
        private CreateApprenticeshipRequest _expectedRequest;
        private CreateApprenticeshipOrchestrator _orchestrator;
        private Mock<IValidationExceptionBuilder> _mockValidationExceptionBuilder;
        private string _expectedErrorMessage;
        private Dictionary<string, string> _validHeader
            = new Dictionary<string, string> { { Constants.RequestHeaderNames.ProviderUkprn, providerUkprn.ToString() } };

        [SetUp]
        public async Task SetUp()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            _actualParameters = fixture.Create<ApiTypes.CreateApprenticeshipParameters>();

            _expectedMediatorResponse = fixture.Create<CreateApprenticeshipResponse>();
            _expectedMapperResponse = fixture.Create<ApiTypes.CreateApprenticeshipResponse>();
            _expectedRequest = new CreateApprenticeshipRequest();
            _expectedErrorMessage = fixture.Create<string>();

            _mockRequestMapper = fixture.Freeze<Mock<ICreateApprenticeshipRequestMapper>>(composer => composer.Do(mock => mock
                .Setup(mapper => mapper.MapFromApiParameters(_actualParameters, It.IsAny<int>()))
                .Returns(_expectedRequest)));

            _mockMediator = fixture.Freeze<Mock<IMediator>>(composer => composer.Do(mock => mock
                .Setup(mediator => mediator.Send(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_expectedMediatorResponse)));

            _mockResponseMapper = fixture.Freeze<Mock<ICreateApprenticeshipResponseMapper>>(composer => composer.Do(mock => mock
                .Setup(mapper => mapper.MapToApiResponse(It.IsAny<CreateApprenticeshipResponse>()))
                .Returns(_expectedMapperResponse)));

            _mockValidationExceptionBuilder = fixture.Freeze<Mock<IValidationExceptionBuilder>>(composer => composer.Do(mock => mock
                .Setup(builder => builder.Build(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("", _expectedErrorMessage)
                }))
            ));

            _orchestrator = fixture.Create<CreateApprenticeshipOrchestrator>();

            _actualResponse = await _orchestrator.CreateApprenticeshipAsync(_actualParameters, _validHeader);
        }

        [Test]
        public void AndParametersAreNull_ThenThrowsValidationException()
        {
            Func<Task> action = async () =>
            {
                await _orchestrator.CreateApprenticeshipAsync(null, _validHeader);
            };

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {_expectedErrorMessage}");

            _mockValidationExceptionBuilder.Verify(builder =>
                builder.Build(
                    ErrorCodes.CreateApprenticeship.CreateApprenticeshipParametersIsNull,
                    ErrorMessages.CreateApprenticeship.CreateApprenticeshipParametersIsNull,
                    It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ThenSendCommandToMediator()
        {
            _mockMediator.Verify(mediator => mediator.Send(_expectedRequest, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void ThenInvokeMapperWithMediatorResponse()
        {
            _mockResponseMapper.Verify(mapper => mapper.MapToApiResponse(_expectedMediatorResponse));
        }

        [Test]
        public void ThenReturnResponseFromMapper()
        {
            _actualResponse.Should().BeSameAs(_expectedMapperResponse);
        }

        [Test]
        public void ThenInvokeRequestMapperWithInputParameters()
        {
            _mockRequestMapper.Verify(mapper => mapper.MapFromApiParameters(_actualParameters, providerUkprn));
        }
    }
}