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
    public class WhenGettingLiveVacancyWithCustomWageType
    {
        [TestCase(WageUnit.Weekly)]
        [TestCase(WageUnit.Monthly)]
        [TestCase(WageUnit.Annually)]
        [TestCase(WageUnit.NotApplicable)]
        public async Task ShouldHaveWageSetForVacanciesWithCustomWageType(WageUnit wageUnit)
        {
            const int weeklyWage = 2550;
            const int vacancyReference = 1234;
            const int liveVacancyStatusId = 2;

            var mockMediator = new Mock<IMediator>();
            var provideSettings = new Mock<IProvideSettings>();
            var sut = new GetApprenticeshipVacancyOrchestrator(mockMediator.Object, provideSettings.Object);

            mockMediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetApprenticeshipVacancyResponse
                {
                    ApprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                            .With(v => v.VacancyReferenceNumber, vacancyReference)
                                            .With(v => v.VacancyStatusId, liveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)WageType.Custom)
                                            .With(v => v.WeeklyWage, weeklyWage)
                                            .Without(v => v.WageText)
                                            .With(v => v.WageUnitId, (int)wageUnit)
                                            .Create()
                });

            var result = await sut.GetApprenticeshipVacancyDetailsAsync(vacancyReference);

            result.VacancyReference.Should().Be(vacancyReference);
            result.WageUnit.Should().Be(wageUnit);
            result.WageText.Should().Be("£2,550.00");
        }
    }
}
