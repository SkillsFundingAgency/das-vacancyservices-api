using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Api.Core.Validation;
using Esfa.Vacancy.Application.Queries.GetTraineeshipVacancy;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Infrastructure.Settings;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Api.Orchestrators;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.GetTraineeshipVacancy.Api.Orchestrators.GivenAGetTraineeshipVacancyOrchestrator
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class WhenGettingLiveTraineeship
    {
        private const int VacancyReference = 1234;
        private const int LiveVacancyStatusId = 2;
        private Mock<IMediator> _mockMediator;
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
        public async Task ThenCreatesGetApprenticeshipVacancyRequestWithRefNumber()
        {
            var uniqueVacancyRef = _fixture.Create<int>();
            await _sut.GetTraineeshipVacancyDetailsAsync(uniqueVacancyRef.ToString());

            _mockMediator.Verify(mediator =>
                mediator.Send(
                    It.Is<GetTraineeshipVacancyRequest>(request => request.Reference == uniqueVacancyRef),
                    CancellationToken.None));
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

            var sut = new GetTraineeshipVacancyOrchestrator(_mockMediator.Object, _fixture.Create<TraineeshipMapper>(), _fixture.Create<IValidationExceptionBuilder>());
            var result = await sut.GetTraineeshipVacancyDetailsAsync(VacancyReference.ToString());

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

            var sut = new GetTraineeshipVacancyOrchestrator(_mockMediator.Object, _fixture.Create<TraineeshipMapper>(), _fixture.Create<IValidationExceptionBuilder>());
            var result = await sut.GetTraineeshipVacancyDetailsAsync(VacancyReference.ToString());

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
            var sut = new GetTraineeshipVacancyOrchestrator(_mockMediator.Object, new TraineeshipMapper(mockProvideSettings.Object), _fixture.Create<IValidationExceptionBuilder>());

            //Act
            var vacancy = await sut.GetTraineeshipVacancyDetailsAsync(VacancyReference.ToString());

            //Assert
            Assert.AreEqual($"{baseUrl}/{VacancyReference}", vacancy.VacancyUrl);
        }

        [Test]
        public void AndParamIsNotAnInt32_ThenThrowsValidationException()
        {
            Func<Task> action = async () =>
            {
                await _sut.GetTraineeshipVacancyDetailsAsync(Guid.NewGuid().ToString());
            };

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {_expectedErrorMessage}");

            _mockValidationExceptionBuilder.Verify(builder =>
                builder.Build(
                    ErrorCodes.GetTraineeship.VacancyReferenceNumberNotInt32,
                    ErrorMessages.GetTraineeship.VacancyReferenceNumberNotNumeric,
                    It.IsAny<string>()), Times.Once);
        }
    }
}
