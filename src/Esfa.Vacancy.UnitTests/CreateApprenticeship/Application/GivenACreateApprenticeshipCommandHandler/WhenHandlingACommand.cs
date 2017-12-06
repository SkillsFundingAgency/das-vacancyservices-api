using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipCommandHandler
{
    [TestFixture]
    public class WhenHandlingACommand
    {
        [Test]
        public async Task ThenItShouldReturnATaskWithDefaultRefNumber()
        {
            var handler = new CreateApprenticeshipCommandHandler();

            var response = await handler.Handle(new CreateApprenticeshipRequest());

            response.VacancyReferenceNumber.Should().Be(0);
        }
    }
}