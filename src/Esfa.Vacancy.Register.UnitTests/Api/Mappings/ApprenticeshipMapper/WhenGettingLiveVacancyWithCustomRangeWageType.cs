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
    public class WhenGettingLiveVacancyWithCustomRangeWageType
    {
        [TestCase(WageUnit.Weekly, 14000, 16000, "£14,000.00 - £16,000.00")]
        [TestCase(WageUnit.Weekly, null, 16000, "Unknown - £16,000.00")]
        [TestCase(WageUnit.Weekly, 14000, null, "£14,000.00 - Unknown")]
        [TestCase(WageUnit.Monthly, 14000, 16000, "£14,000.00 - £16,000.00")]
        [TestCase(WageUnit.Monthly, null, 16000, "Unknown - £16,000.00")]
        [TestCase(WageUnit.Monthly, 14000, null, "£14,000.00 - Unknown")]
        [TestCase(WageUnit.Annually, 14000, 16000, "£14,000.00 - £16,000.00")]
        [TestCase(WageUnit.Annually, null, 16000, "Unknown - £16,000.00")]
        [TestCase(WageUnit.Annually, 14000, null, "£14,000.00 - Unknown")]
        public async Task ShouldHaveWageSetForVacanciesWithCustomRangeWageType(WageUnit wageUnit, decimal? lowerBound, decimal? upperBound, string expectedWageText)
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
                                            .With(v => v.WageLowerBound, lowerBound)
                                            .With(v => v.WageUpperBound, upperBound)
                                            .With(v => v.WageType, (int)WageType.CustomRange)
                                            .With(v => v.WeeklyWage, weeklyWage)
                                            .Without(v => v.WageText)
                                            .With(v => v.WageUnitId, (int)wageUnit)
                                            .Create()
                });

            var result = await sut.GetApprenticeshipVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.WageUnit.Should().Be(wageUnit);
            result.WageText.Should().Be(expectedWageText);
        }
    }
}
