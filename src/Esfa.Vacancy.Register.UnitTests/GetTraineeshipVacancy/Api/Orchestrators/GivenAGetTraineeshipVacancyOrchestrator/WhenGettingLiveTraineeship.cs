using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Esfa.Vacancy.Register.Application.Queries.GetTraineeshipVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.GetTraineeshipVacancy.Api.Orchestrators.GivenAGetTraineeshipVacancyOrchestrator
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class WhenGettingLiveTraineeship
    {
        private const int VacancyReference = 1234;
        private const int LiveVacancyStatusId = 2;
        private Mock<IMediator> _mockMediator;
        private Mock<IProvideSettings> _mockProvideSettings;
        private GetTraineeshipVacancyOrchestrator _sut;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _mockProvideSettings = new Mock<IProvideSettings>();
            _sut = new GetTraineeshipVacancyOrchestrator(_mockMediator.Object, _mockProvideSettings.Object);
        }

        [Test]
        public async Task WithNonAnonymousEmployer_ShouldNotReplaceEmployerNameAndDescription()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetTraineeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetTraineeshipVacancyResponse
                {
                    TraineeshipVacancy = new Fixture().Build<Domain.Entities.TraineeshipVacancy>()
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

            var result = await _sut.GetTraineeshipVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.EmployerName.Should().Be("ABC Ltd");
            result.EmployerDescription.Should().Be("A plain company");
            result.EmployerWebsite.Should().Be("http://www.google.co.uk");
            result.Location.Should().NotBeNull();
            result.Location.AddressLine1.Should().NotBeNull();
            result.Location.AddressLine2.Should().NotBeNull();
            result.Location.AddressLine3.Should().NotBeNull();
            result.Location.AddressLine4.Should().NotBeNull();
            result.Location.AddressLine5.Should().NotBeNull();
            result.Location.Town.Should().NotBeNull();
            result.Location.PostCode.Should().NotBeNull();
            result.Location.GeoPoint.Should().NotBeNull();
            result.Location.GeoPoint.Longitude.Should().NotBeNull();
            result.Location.GeoPoint.Latitude.Should().NotBeNull();
        }

        [Test]
        public async Task WithAnonymousEmployer_ShouldReplaceEmployerNameAndDescription()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetTraineeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetTraineeshipVacancyResponse
                {
                    TraineeshipVacancy = new Fixture().Build<Domain.Entities.TraineeshipVacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.EmployerName, "Her Majesties Secret Service")
                                            .With(v => v.EmployerDescription, "A private description")
                                            .With(v => v.AnonymousEmployerName, "ABC Ltd")
                                            .With(v => v.AnonymousEmployerDescription, "A plain company")
                                            .With(v => v.AnonymousEmployerReason, "Because I want to test")
                                            .Create()
                });

            var result = await _sut.GetTraineeshipVacancyDetailsAsync(VacancyReference);

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
            result.Location.GeoPoint.Should().BeNull();
            result.Location.Town.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public async Task ShouldPopulateTraineeshipVacancyUrl()
        {
            //Arrange
            var baseUrl = "https://findapprentice.com/traineeship/reference";

            _mockProvideSettings.Setup(p => p.GetSetting(ApplicationSettingKeyConstants.LiveTraineeshipVacancyBaseUrlKey))
                                .Returns(baseUrl);

            var response = new GetTraineeshipVacancyResponse
            {
                TraineeshipVacancy = new Fixture().Build<Domain.Entities.TraineeshipVacancy>()
                                                    .With(v => v.VacancyReferenceNumber, VacancyReference)
                                                    .Create()
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetTraineeshipVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            var vacancy = await _sut.GetTraineeshipVacancyDetailsAsync(VacancyReference);

            //Assert
            Assert.AreEqual($"{baseUrl}/{VacancyReference}", vacancy.VacancyUrl);
        }
    }
}
