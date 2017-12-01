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
        private ApiTypes.CreateApprecticeshipResponse _actualResponse;
        private CreateApprenticeshipResponse _expectedMediatorResponse;
        private ApiTypes.CreateApprecticeshipResponse _expectedMapperResponse;
        private Mock<ICreateApprenticeshipResponseMapper> _mockMapper;

        [SetUp]
        public async Task SetUp()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            _expectedMediatorResponse = fixture.Create<CreateApprenticeshipResponse>();
            _expectedMapperResponse = fixture.Create<ApiTypes.CreateApprecticeshipResponse>();

            _mockMediator = fixture.Freeze<Mock<IMediator>>(composer => composer.Do(mock => mock
                .Setup(mediator => mediator.Send(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_expectedMediatorResponse)));

            _mockMapper = fixture.Freeze<Mock<ICreateApprenticeshipResponseMapper>>(composer => composer.Do(mock => mock
                .Setup(mapper => mapper.MapToApiResponse(It.IsAny<CreateApprenticeshipResponse>()))
                .Returns(_expectedMapperResponse)));

            var orchestrator = fixture.Create<CreateApprenticeshipOrchestrator>();

            _actualResponse = await orchestrator.CreateApprecticeship(new ApiTypes.CreateApprenticeshipParameters());
        }

        [Test]
        public void ShouldSendCommandToMediator()
        {
            _mockMediator.Verify(mediator => mediator.Send(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void ShouldInvokeMapperWithMediatorResponse()
        {
            _mockMapper.Verify(mapper => mapper.MapToApiResponse(_expectedMediatorResponse));
        }

        [Test]
        public void ShouldReturnResponseFromMapper()
        {
            _actualResponse.Should().BeSameAs(_expectedMapperResponse);
        }
    }
}