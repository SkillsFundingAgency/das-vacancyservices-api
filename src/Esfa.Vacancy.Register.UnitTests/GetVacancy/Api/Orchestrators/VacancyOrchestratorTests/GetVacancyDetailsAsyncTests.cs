using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Api.App_Start;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.GetVacancy.Api.Orchestrators.VacancyOrchestratorTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class GetVacancyDetailsAsyncTests
    {
        private const int VacancyReference = 1234;
        private const int LiveVacancyStatusId = 2;
        private Mock<IMediator> _mockMediator;
        private Mock<IProvideSettings> _provideSettings;
        private GetVacancyOrchestrator _sut;
        private IMapper _mapper;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _provideSettings = new Mock<IProvideSettings>();
            _sut = new GetVacancyOrchestrator(_mockMediator.Object, _provideSettings.Object, _mapper);
        }

        [Test]
        public async Task GetLiveNonAnonymousEmployerVacancy_ShouldNotReplaceEmployerNameAndDescription()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetApprenticeshipVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
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
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
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
    }
}
