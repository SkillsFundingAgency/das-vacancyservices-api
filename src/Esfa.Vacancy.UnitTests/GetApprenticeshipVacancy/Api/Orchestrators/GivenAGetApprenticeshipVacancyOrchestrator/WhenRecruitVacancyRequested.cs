using System.Threading;
using Esfa.Vacancy.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Api.Orchestrators;
using MediatR;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using SFA.DAS.Recruit.Vacancies.Client;
using SFA.DAS.Recruit.Vacancies.Client.Entities;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Orchestrators.GivenAGetApprenticeshipVacancyOrchestrator
{
    [TestFixture]
    public class WhenRecruitVacancyRequested
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IClient> _mockClient;
        private Mock<IApprenticeshipMapper> _mockMapper;
        private Mock<IRecruitVacancyMapper> _recuitMapperMock;

        [SetUp]
        public void Initialise()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            _mockMediator = fixture.Freeze<Mock<IMediator>>();
            _mockClient = fixture.Freeze<Mock<IClient>>();
            _mockMapper = fixture.Freeze<Mock<IApprenticeshipMapper>>();
            _recuitMapperMock = fixture.Freeze<Mock<IRecruitVacancyMapper>>();

            var sut = fixture.Create<GetApprenticeshipVacancyOrchestrator>();

            sut.GetApprenticeshipVacancyDetailsAsync("1234567890").Wait();
        }

        [Test]
        public void ThenMediatorIsNotInvoked()
        {
            _mockMediator
                .Verify(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), It.IsAny<CancellationToken>()), Times.Never);

            _mockMapper.Verify(m => m.MapToApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Never);
        }

        [Test]
        public void ThenRecruitClientIsInvoked()
        {
            _mockClient.Verify(m => m.GetVacancy(It.IsAny<long>()));
            _recuitMapperMock.Verify(m => m.MapFromRecruitVacancy(It.IsAny<SFA.DAS.Recruit.Vacancies.Client.Entities.Vacancy>()));
        }
    }
}