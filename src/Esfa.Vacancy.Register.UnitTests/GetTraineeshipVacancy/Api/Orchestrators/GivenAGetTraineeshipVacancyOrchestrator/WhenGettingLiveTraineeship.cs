using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Esfa.Vacancy.Register.Api.Validation;
using Esfa.Vacancy.Register.Application.Queries.GetTraineeshipVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

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
        private IFixture _fixture;
        private string _expectedErrorMessage;
        private Mock<IValidationExceptionBuilder> _mockValidationExceptionBuilder;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _expectedErrorMessage = _fixture.Create<string>();

            _mockMediator = _fixture.Freeze<Mock<IMediator>>(composer => composer.Do(mock => mock
                .Setup(mediator => mediator.Send(It.IsAny<GetTraineeshipVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_fixture.Create<GetTraineeshipVacancyResponse>())));

            _mockValidationExceptionBuilder = _fixture.Freeze<Mock<IValidationExceptionBuilder>>(composer => composer.Do(mock => mock
                .Setup(builder => builder.Build(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("", _expectedErrorMessage)
                }))
            ));

            _sut = _fixture.Create<GetTraineeshipVacancyOrchestrator>();
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

            var sut = new GetTraineeshipVacancyOrchestrator(_mockMediator.Object, _fixture.Create<TraineeshipMapper>());
            var result = await sut.GetTraineeshipVacancyDetailsAsync(VacancyReference);

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

            var sut = new GetTraineeshipVacancyOrchestrator(_mockMediator.Object, _fixture.Create<TraineeshipMapper>());
            var result = await sut.GetTraineeshipVacancyDetailsAsync(VacancyReference);

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

            var mockProvideSettings = new Mock<IProvideSettings>();
            mockProvideSettings
                .Setup(p => p.GetSetting(ApplicationSettingKeyConstants.LiveTraineeshipVacancyBaseUrlKey))
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
            var sut = new GetTraineeshipVacancyOrchestrator(_mockMediator.Object, new TraineeshipMapper(mockProvideSettings.Object));

            //Act
            var vacancy = await sut.GetTraineeshipVacancyDetailsAsync(VacancyReference);

            //Assert
            Assert.AreEqual($"{baseUrl}/{VacancyReference}", vacancy.VacancyUrl);
        }
    }
}
