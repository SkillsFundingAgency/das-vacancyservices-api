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
        private const int VacancyReference = 1234;
        private Mock<IMediator> _mockMediator;
        private Mock<IProvideSettings> _provideSettings;
        private VacancyOrchestrator _sut;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            AutoMapperConfig.Configure();
        }

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _provideSettings = new Mock<IProvideSettings>();
            _sut = new VacancyOrchestrator(_mockMediator.Object, _provideSettings.Object);
        }

        [Test]
        public async Task GetLiveNonAnonymousEmployerVacancy_ShouldNotReplaceEmployerNameAndDescription()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, 2)
                                            .With(v => v.EmployerName, "ABC Ltd")
                                            .With(v => v.EmployerDescription, "A plain company")
                                            .With(v => v.EmployerWebsite, "http://www.google.co.uk")
                                            .Without(v => v.AnonymousEmployerName)
                                            .Without(v => v.AnonymousEmployerDescription)
                                            .Without(v => v.AnonymousEmployerReason)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.EmployerName.Should().Be("ABC Ltd");
            result.EmployerDescription.Should().Be("A plain company");
            result.EmployerWebsite.Should().Be("http://www.google.co.uk");
            result.VacancyLocation.Should().NotBe(null);
            result.VacancyLocation.AddressLine1.Should().NotBe(null);
            result.VacancyLocation.AddressLine2.Should().NotBe(null);
            result.VacancyLocation.AddressLine3.Should().NotBe(null);
            result.VacancyLocation.AddressLine4.Should().NotBe(null);
            result.VacancyLocation.AddressLine5.Should().NotBe(null);
            result.VacancyLocation.Town.Should().NotBe(null);
            result.VacancyLocation.PostCode.Should().NotBe(null);
            result.VacancyLocation.Longitude.Should().NotBe(null);
            result.VacancyLocation.Latitude.Should().NotBe(null);
        }

        [Test]
        public async Task GetLiveAnonymousEmployerVacancy_ShouldReplaceEmployerNameAndDescription()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, 2)
                                            .With(v => v.EmployerName, "Her Majesties Secret Service")
                                            .With(v => v.EmployerDescription, "A private description")
                                            .With(v => v.AnonymousEmployerName, "ABC Ltd")
                                            .With(v => v.AnonymousEmployerDescription, "A plain company")
                                            .With(v => v.AnonymousEmployerReason, "Because I want to test")
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.EmployerName.Should().Be("ABC Ltd");
            result.EmployerDescription.Should().Be("A plain company");
            result.EmployerWebsite.Should().BeNullOrEmpty();
            result.VacancyLocation.AddressLine1.Should().BeNull();
            result.VacancyLocation.AddressLine2.Should().BeNull();
            result.VacancyLocation.AddressLine3.Should().BeNull();
            result.VacancyLocation.AddressLine4.Should().BeNull();
            result.VacancyLocation.AddressLine5.Should().BeNull();
            result.VacancyLocation.PostCode.Should().BeNull();
            result.VacancyLocation.Latitude.Should().BeNull();
            result.VacancyLocation.Longitude.Should().BeNull();
            result.VacancyLocation.Town.Should().NotBeNullOrWhiteSpace();
        }
    }
}
