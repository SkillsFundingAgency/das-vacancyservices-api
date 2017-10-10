using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.App_Start;
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
    public class WhenGettingLiveVacancyWithCustomRangeWageType
    {
        private const int VacancyReference = 1234;
        private const int LiveVacancyStatusId = 2;
        private Mock<IMediator> _mockMediator;
        private Mock<IProvideSettings> _provideSettings;
        private GetVacancyOrchestrator _sut;

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
            _sut = new GetVacancyOrchestrator(_mockMediator.Object, _provideSettings.Object);
        }

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

            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
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

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Apprenticeship);
            result.WageUnit.Should().Be(wageUnit);
            result.WageText.Should().Be(expectedWageText);
        }
    }
}
