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
    public class WhenGettingLiveVacancyWithAmbiguousWage
    {
        [TestCase(WageType.Unwaged, "Unwaged")]
        [TestCase(WageType.ToBeAgreedUponAppointment, "To be agreed upon appointment")]
        [TestCase(WageType.CompetitiveSalary, "Competitive salary")]
        public async Task ShouldHaveAppropriateWageDescription(WageType wageType, string expectedWageText)
        {
            const int vacancyReference = 1234;
            const int liveVacancyStatusId = 2;

            var mockMediator = new Mock<IMediator>();
            var provideSettings = new Mock<IProvideSettings>();
            var sut = new GetApprenticeshipVacancyOrchestrator(mockMediator.Object, provideSettings.Object);

            mockMediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetApprenticeshipVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, vacancyReference)
                                            .With(v => v.VacancyStatusId, liveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)wageType)
                                            .Without(v => v.WeeklyWage)
                                            .Without(v => v.WageUnitId)
                                            .Create()
                });

            var result = await sut.GetApprenticeshipVacancyDetailsAsync(vacancyReference);

            result.VacancyReference.Should().Be(vacancyReference);
            result.WageUnit.Should().BeNull();
            result.WageText.Should().Be(expectedWageText);
        }
    }
}
