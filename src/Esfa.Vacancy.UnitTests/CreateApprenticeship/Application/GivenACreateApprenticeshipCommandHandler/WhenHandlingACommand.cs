using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Repositories;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipCommandHandler
{
    [TestFixture]
    public class WhenHandlingACommand
    {
        private Mock<IVacancyRepository> _mockVacancyRepository;

        [Test]
        public async Task ThenItShouldReturnATaskWithDefaultRefNumber()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var expectedRefNumber = fixture.Create<int>();

            _mockVacancyRepository = fixture.Freeze<Mock<IVacancyRepository>>(composer => composer.Do(mock => mock
                .Setup(repository => repository.CreateApprenticeshipAsync(It.IsAny<CreateApprenticeshipParameters>()))
                .ReturnsAsync(expectedRefNumber)));

            var handler = fixture.Create<CreateApprenticeshipCommandHandler>();

            var response = await handler.Handle(new CreateApprenticeshipRequest());

            response.VacancyReferenceNumber.Should().Be(expectedRefNumber);
        }
    }
}