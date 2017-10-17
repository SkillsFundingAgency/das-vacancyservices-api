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
    public class WhenGettingLiveVacancyWithLegacyTextWageType
    {
        [Test]
        public async Task ShouldHaveUnknownWageForVacanciesWithLegacyTextWageType()
        {
            const int vacancyReference = 1234;
            const int liveVacancyStatusId = 2;
            const string unknownwWageText = "Unknown";

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
                                            .With(v => v.WageType, (int)WageType.LegacyText)
                                            .Without(v => v.WageUnitId)
                                            .Create()
                });

            var result = await sut.GetApprenticeshipVacancyDetailsAsync(vacancyReference);

            result.VacancyReference.Should().Be(vacancyReference);
            result.WageUnit.Should().BeNull();
            result.WageText.Should().Be(unknownwWageText);
        }
    }
}
