using System;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Esfa.Vacancy.Register.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using FluentValidation;
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
        private Mock<IApprenticeshipMapper> _mockApprenticeshipMapper;
        private GetApprenticeshipVacancyOrchestrator _sut;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _mockMediator
                .Setup(mediator => mediator.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetApprenticeshipVacancyResponse());

            _mockApprenticeshipMapper = new Mock<IApprenticeshipMapper>();
            _mockApprenticeshipMapper
                .Setup(mapper => mapper.MapToApprenticeshipVacancy(It.IsAny<Domain.Entities.ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy());

            _provideSettings = new Mock<IProvideSettings>();

            _sut = new GetApprenticeshipVacancyOrchestrator(_mockMediator.Object, _mockApprenticeshipMapper.Object);
        }

        [Test]
        public async Task ThenCreatesGetApprenticeshipVacancyRequestWithRefNumber()
        {
            var uniqueVacancyRef = new Random().Next();
            await _sut.GetApprenticeshipVacancyDetailsAsync(uniqueVacancyRef.ToString());
            _mockMediator.Verify(mediator => 
                mediator.Send(It.Is<GetApprenticeshipVacancyRequest>(request => request.Reference == uniqueVacancyRef), CancellationToken.None));
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

            var sut = new GetApprenticeshipVacancyOrchestrator(_mockMediator.Object, new ApprenticeshipMapper(_provideSettings.Object));
            var result = await sut.GetApprenticeshipVacancyDetailsAsync(VacancyReference.ToString());

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

            var sut = new GetApprenticeshipVacancyOrchestrator(_mockMediator.Object, new ApprenticeshipMapper(_provideSettings.Object));
            var result = await sut.GetApprenticeshipVacancyDetailsAsync(VacancyReference.ToString());

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

            var sut = new GetApprenticeshipVacancyOrchestrator(_mockMediator.Object, new ApprenticeshipMapper(_provideSettings.Object));
            //Act
            var vacancy = await sut.GetApprenticeshipVacancyDetailsAsync("12345");

            //Assert
            Assert.AreEqual($"{baseUrl}/{VacancyReference}", vacancy.VacancyUrl);
        }

        [Test]
        public void AndParamIsNotAnInt32_ThenThrowsValidationException()
        {
            Func<Task> action = async () =>
            {
                await _sut.GetApprenticeshipVacancyDetailsAsync(Guid.NewGuid().ToString());
            };

            action.ShouldThrow<ValidationException>();
            //WithMessage($"Validation failed: \r\n -- {ErrorMessages.SearchApprenticeships.SearchApprenticeshipParametersIsNull}");
        }
    }
}
