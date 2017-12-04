using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Manage.Api.Mappings;
using Esfa.Vacancy.Manage.Api.Orchestrators;
using FluentAssertions;
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
        private Mock<IMediator> _mockMediator;
        private ApiTypes.CreateApprenticeshipResponse _actualResponse;
        private CreateApprenticeshipResponse _expectedMediatorResponse;
        private ApiTypes.CreateApprenticeshipResponse _expectedMapperResponse;
        private Mock<ICreateApprenticeshipResponseMapper> _mockResponseMapper;
        private Mock<ICreateApprenticeshipRequestMapper> _mockRequestMapper;
        private ApiTypes.CreateApprenticeshipParameters _actualParameters;
        private CreateApprenticeshipRequest _expectedRequest;

        [SetUp]
        public async Task SetUp()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            _actualParameters = fixture.Create<ApiTypes.CreateApprenticeshipParameters>();

            _expectedMediatorResponse = fixture.Create<CreateApprenticeshipResponse>();
            _expectedMapperResponse = fixture.Create<ApiTypes.CreateApprenticeshipResponse>();
            _expectedRequest = new CreateApprenticeshipRequest();

            _mockRequestMapper = fixture.Freeze<Mock<ICreateApprenticeshipRequestMapper>>(composer => composer.Do(mock => mock
                .Setup(mapper => mapper.MapFromApiParameters(_actualParameters))
                .Returns(_expectedRequest)));

            _mockMediator = fixture.Freeze<Mock<IMediator>>(composer => composer.Do(mock => mock
                .Setup(mediator => mediator.Send(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_expectedMediatorResponse)));

            _mockResponseMapper = fixture.Freeze<Mock<ICreateApprenticeshipResponseMapper>>(composer => composer.Do(mock => mock
                .Setup(mapper => mapper.MapToApiResponse(It.IsAny<CreateApprenticeshipResponse>()))
                .Returns(_expectedMapperResponse)));

            var orchestrator = fixture.Create<CreateApprenticeshipOrchestrator>();

            _actualResponse = await orchestrator.CreateApprecticeshipAsync(_actualParameters);
        }

        [Test]
        public void ShouldSendCommandToMediator()
        {
            _mockMediator.Verify(mediator => mediator.Send(_expectedRequest, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void ShouldInvokeMapperWithMediatorResponse()
        {
            _mockResponseMapper.Verify(mapper => mapper.MapToApiResponse(_expectedMediatorResponse));
        }

        [Test]
        public void ShouldReturnResponseFromMapper()
        {
            _actualResponse.Should().BeSameAs(_expectedMapperResponse);
        }

        [Test]
        public void ShouldInvokeRequestMapperWithInputParameters()
        {
            _mockRequestMapper.Verify(mapper => mapper.MapFromApiParameters(_actualParameters));
        }
    }
}