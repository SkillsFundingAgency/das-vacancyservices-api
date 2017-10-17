using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.Api.Orchestrators.VacancyOrchestratorTests
{
    [TestFixture]
    public class WhenGettingLiveVacancyWage
    {
        [TestCase(1, WageUnit.NotApplicable)]
        [TestCase(2, WageUnit.Weekly)]
        [TestCase(3, WageUnit.Monthly)]
        [TestCase(4, WageUnit.Annually)]
        [TestCase(null, null)]
        public async Task ShouldMapWageUnitEnum(int? wageUnitId, WageUnit? wageUnitType)
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            var provideSettings = new Mock<IProvideSettings>();
            var sut = new GetApprenticeshipVacancyOrchestrator(mockMediator.Object, provideSettings.Object);

            var response = new GetApprenticeshipVacancyResponse
            {
                ApprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                        .With(v => v.WageType, (int) WageType.Custom)
                                        .With(v => v.WageUnitId, wageUnitId)
                                        .Create()
            };

            mockMediator
                .Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var vacancy = await sut.GetApprenticeshipVacancyDetailsAsync(12345);
            //Assert
            vacancy.WageUnit.ShouldBeEquivalentTo(wageUnitType);
        }
    }
}
