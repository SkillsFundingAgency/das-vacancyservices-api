using System;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Api.App_Start;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.Api.Orchestrators.VacancyOrchestratorTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class GetVacancyDetailsAsyncTests
    {
        private string _vacancyReferenceParameter;
        private const int LiveVacancyStatusId = 2;
        private Mock<IMediator> _mockMediator;
        private Mock<IProvideSettings> _provideSettings;
        private VacancyOrchestrator _sut;
        private int _vacancyReferenceNumber;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            AutoMapperConfig.Configure();
        }

        [SetUp]
        public void GivenAVacancyOrchestrator()
        {
            _vacancyReferenceNumber = new Random().Next();
            _vacancyReferenceParameter = _vacancyReferenceNumber.ToString();

            _mockMediator = new Mock<IMediator>();
            _mockMediator
                .Setup(m => m.Send(
                    It.Is<GetVacancyRequest>(request => request.Reference == _vacancyReferenceNumber), 
                    CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Domain.Entities.Vacancy
                    {
                        VacancyReferenceNumber = _vacancyReferenceNumber
                    }
                });

            _provideSettings = new Mock<IProvideSettings>();

            _sut = new VacancyOrchestrator(_mockMediator.Object, _provideSettings.Object);
        }

        [Test]
        public async Task WhenGettingLiveNonAnonymousEmployerVacancy_ShouldNotReplaceEmployerNameAndDescription()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, _vacancyReferenceNumber)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.EmployerName, "ABC Ltd")
                                            .With(v => v.EmployerDescription, "A plain company")
                                            .With(v => v.EmployerWebsite, "http://www.google.co.uk")
                                            .Without(v => v.AnonymousEmployerName)
                                            .Without(v => v.AnonymousEmployerDescription)
                                            .Without(v => v.AnonymousEmployerReason)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(_vacancyReferenceParameter);

            result.VacancyReference.Should().Be(_vacancyReferenceNumber);
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
        public async Task WhenGettingLiveAnonymousEmployerVacancy_ShouldReplaceEmployerNameAndDescription()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, _vacancyReferenceNumber)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.EmployerName, "Her Majesties Secret Service")
                                            .With(v => v.EmployerDescription, "A private description")
                                            .With(v => v.AnonymousEmployerName, "ABC Ltd")
                                            .With(v => v.AnonymousEmployerDescription, "A plain company")
                                            .With(v => v.AnonymousEmployerReason, "Because I want to test")
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(_vacancyReferenceParameter);

            result.VacancyReference.Should().Be(_vacancyReferenceNumber);
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
        public async Task WhenCallingGetVacancyDetailsAsync_AndReferenceIsParsableAsInt_ThenParsesInt()
        {
            var result = await _sut.GetVacancyDetailsAsync(_vacancyReferenceParameter);

            Assert.That(result.VacancyReference, Is.EqualTo(_vacancyReferenceNumber));
        }

        [Test]
        public async Task WhenCallingGetVacancyDetailsAsync_AndReferenceContainsVAC_ThenParsesInt()
        {
            var result = await _sut.GetVacancyDetailsAsync($"VAC00{_vacancyReferenceParameter}");

            Assert.That(result.VacancyReference, Is.EqualTo(_vacancyReferenceNumber));
        }
    }
}
