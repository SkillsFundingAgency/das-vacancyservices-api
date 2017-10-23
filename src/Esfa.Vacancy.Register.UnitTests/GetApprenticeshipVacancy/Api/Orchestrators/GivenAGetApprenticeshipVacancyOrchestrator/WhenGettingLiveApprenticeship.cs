using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Esfa.Vacancy.Register.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.GetApprenticeshipVacancy.Api.Orchestrators.GivenAGetApprenticeshipVacancyOrchestrator
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class WhenGettingLiveApprenticeship
    {
        private const int VacancyReference = 1234;
        private const int LiveVacancyStatusId = 2;
        private Mock<IMediator> _mockMediator;
        private Mock<IProvideSettings> _provideSettings;
        private GetApprenticeshipVacancyOrchestrator _sut;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _provideSettings = new Mock<IProvideSettings>();

            _sut = new GetApprenticeshipVacancyOrchestrator(_mockMediator.Object, _provideSettings.Object);
        }

        [Test]
        public async Task GetLiveNonAnonymousEmployerVacancy_ShouldNotReplaceEmployerNameAndDescription()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetApprenticeshipVacancyResponse
                {
                    ApprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                            .With(v => v.WageUnitId, null)
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.EmployerName, "ABC Ltd")
                                            .With(v => v.EmployerDescription, "A plain company")
                                            .With(v => v.EmployerWebsite, "http://www.google.co.uk")
                                            .Without(v => v.AnonymousEmployerName)
                                            .Without(v => v.AnonymousEmployerDescription)
                                            .Without(v => v.AnonymousEmployerReason)
                                            .Create()
                });

            var result = await _sut.GetApprenticeshipVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.EmployerName.Should().Be("ABC Ltd");
            result.EmployerDescription.Should().Be("A plain company");
            result.EmployerWebsite.Should().Be("http://www.google.co.uk");
            result.Location.Should().NotBe(null);
            result.Location.AddressLine1.Should().NotBe(null);
            result.Location.AddressLine2.Should().NotBe(null);
            result.Location.AddressLine3.Should().NotBe(null);
            result.Location.AddressLine4.Should().NotBe(null);
            result.Location.AddressLine5.Should().NotBe(null);
            result.Location.Town.Should().NotBe(null);
            result.Location.PostCode.Should().NotBe(null);
            result.Location.Longitude.Should().NotBe(null);
            result.Location.Latitude.Should().NotBe(null);
        }

        [Test]
        public async Task GetLiveAnonymousEmployerVacancy_ShouldReplaceEmployerNameAndDescription()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetApprenticeshipVacancyResponse
                {
                    ApprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                            .With(v => v.WageUnitId, null)
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.EmployerName, "Her Majesties Secret Service")
                                            .With(v => v.EmployerDescription, "A private description")
                                            .With(v => v.AnonymousEmployerName, "ABC Ltd")
                                            .With(v => v.AnonymousEmployerDescription, "A plain company")
                                            .With(v => v.AnonymousEmployerReason, "Because I want to test")
                                            .Create()
                });

            var result = await _sut.GetApprenticeshipVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.EmployerName.Should().Be("ABC Ltd");
            result.EmployerDescription.Should().Be("A plain company");
            result.EmployerWebsite.Should().BeNullOrEmpty();
            result.Location.AddressLine1.Should().BeNull();
            result.Location.AddressLine2.Should().BeNull();
            result.Location.AddressLine3.Should().BeNull();
            result.Location.AddressLine4.Should().BeNull();
            result.Location.AddressLine5.Should().BeNull();
            result.Location.PostCode.Should().BeNull();
            result.Location.Latitude.Should().BeNull();
            result.Location.Longitude.Should().BeNull();
            result.Location.Town.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task ShouldPopulateVacancyUrl()
        {
            //Arrange
            var baseUrl = "https://findapprentice.com/apprenticeship/reference";

            _provideSettings.Setup(p => p.GetSetting(ApplicationSettingKeyConstants.LiveApprenticeshipVacancyBaseUrlKey)).Returns(baseUrl);
            
            var response = new GetApprenticeshipVacancyResponse()
            {
                ApprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                            .With(v => v.WageUnitId, null)
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .Create()
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var sut = new GetApprenticeshipVacancyOrchestrator(_mockMediator.Object, _provideSettings.Object);
            //Act
            var vacancy = await sut.GetApprenticeshipVacancyDetailsAsync(12345);

            //Assert
            Assert.AreEqual($"{baseUrl}/{VacancyReference}", vacancy.VacancyUrl);
        }
    }
}
