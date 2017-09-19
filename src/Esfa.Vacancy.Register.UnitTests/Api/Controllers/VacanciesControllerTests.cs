using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Esfa.Vacancy.Register.Api.Controllers;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.UnitTests.Api.Controllers
{
    [TestFixture]
    public class VacanciesControllerTests
    {
        [Test]
        public async Task GivenAVacanciesController_WhenCallingGet_ThenReturnsCorrectVacancy()
        {
            var expectedVacancy = new Vacancy.Api.Types.Vacancy();

            var mockLog = new Mock<ILog>();

            var mockOrchestrator = new Mock<IVacancyOrchestrator>();            
            mockOrchestrator
                .Setup(orchestrator => orchestrator.GetVacancyDetailsAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedVacancy);

            var controller = new VacanciesController(mockLog.Object, mockOrchestrator.Object);

            var result = await controller.Get("2345") as OkNegotiatedContentResult<Vacancy.Api.Types.Vacancy>;

            Assert.That(result?.Content, Is.SameAs(expectedVacancy));
        }
    }
}