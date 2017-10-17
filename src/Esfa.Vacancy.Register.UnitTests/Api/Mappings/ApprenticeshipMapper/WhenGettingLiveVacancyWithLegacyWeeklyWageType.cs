using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Esfa.Vacancy.Register.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.Api.Mappings.ApprenticeshipMapper
{
    [TestFixture]
    public class WhenGettingLiveVacancyWithLegacyWeeklyWageType
    {
        [Test]
        public async Task LiveVacanciesWithLegacyWeeklyWageTypeShouldHaveWageSetFromWeeklyWage()
        {
            const int weeklyWage = 2550;
            const int VacancyReference = 1234;
            const int LiveVacancyStatusId = 2;

            var mockMediator = new Mock<IMediator>();
            var provideSettings = new Mock<IProvideSettings>();
            var sut = new GetApprenticeshipVacancyOrchestrator(mockMediator.Object, provideSettings.Object);

            mockMediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetApprenticeshipVacancyResponse
                {
                    ApprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)WageType.LegacyWeekly)
                                            .With(v => v.WeeklyWage, weeklyWage)
                                            .Without(v => v.WageText)
                                            .With(v => v.WageUnitId, (int)WageUnit.Weekly)
                                            .Create()
                });

            var result = await sut.GetApprenticeshipVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.WageUnit.Should().Be(WageUnit.Weekly);
            result.WageText.Should().Be("£2,550.00");
        }
    }
}
