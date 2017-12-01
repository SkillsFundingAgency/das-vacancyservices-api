using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Manage.Api.Orchestrators;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Api.Orchestrators.GivenACreateApprenticeshipOrchestrator
{
    [TestFixture]
    public class WhenCreatingAnApprenticeshipVacancy
    {
        [Test]
        public async Task ShouldSendCommandToMediator()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var mockMediator = fixture.Freeze<Mock<IMediator>>();
            var orchestrator = fixture.Create<CreateApprenticeshipOrchestrator>();

            orchestrator.CreateApprecticeship(new CreateApprenticeshipParameters());

            mockMediator.Verify(mediator => mediator.Send(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}