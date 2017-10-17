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
    public class WhenGettingLiveVacancyWithNationalMinimumWageType
    {
        private const int VacancyReference = 1234;
        private const int LiveVacancyStatusId = 2;
        private const string UnknownwWageText = "Unknown";
        private Mock<IMediator> _mockMediator;
        private Mock<IProvideSettings> _provideSettings;
        private GetApprenticeshipVacancyOrchestrator _sut;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _provideSettings = new Mock<IProvideSettings>();

            _sut = new GetApprenticeshipVacancyOrchestrator(_mockMediator.Object, _provideSettings.Object);
        }

        [TestCase(null, 7.05, "Unknown - £211.50")]
        [TestCase(4.05, null, "£121.50 - Unknown")]
        [TestCase(4.05, 7.05, "£121.50 - £211.50")]
        [TestCase(null, null, "Unknown")]
        public async Task ShouldHaveWageSetForVacancy(decimal? lowerBound, decimal? upperBound, string expectedWageText)
        {
            var minNationalWageLowerBound = lowerBound;
            var minNationalWageUpperBound = upperBound;
            const decimal hoursPerWeek = 30;

            _mockMediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetApprenticeshipVacancyResponse
                {
                    ApprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)WageType.NationalMinimum)
                                            .With(v => v.MinimumWageLowerBound, minNationalWageLowerBound)
                                            .With(v => v.MinimumWageUpperBound, minNationalWageUpperBound)
                                            .With(v => v.HoursPerWeek, hoursPerWeek)
                                            .Without(v => v.WageUnitId)
                                            .Create()
                });

            var result = await _sut.GetApprenticeshipVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.WageUnit.Should().BeNull();
            result.WageText.Should().Be(expectedWageText);
        }

        [TestCase(null, 7.05)]
        [TestCase(4.05, null)]
        [TestCase(4.05, 7.05)]
        [TestCase(null, null)]
        public async Task ShouldHaveUnknownWageForVacancyWithUndefinedHoursPerWeek(decimal? lowerBound, decimal? upperBound)
        {
            var minNationalWageLowerBound = lowerBound;
            var minNationalWageUpperBound = upperBound;

            _mockMediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetApprenticeshipVacancyResponse
                {
                    ApprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)WageType.NationalMinimum)
                                            .With(v => v.MinimumWageLowerBound, minNationalWageLowerBound)
                                            .With(v => v.MinimumWageUpperBound, minNationalWageUpperBound)
                                            .Without(v => v.HoursPerWeek)
                                            .Without(v => v.WageUnitId)
                                            .Create()
                });

            var result = await _sut.GetApprenticeshipVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.WageUnit.Should().BeNull();
            result.WageText.Should().Be(UnknownwWageText);
        }

        [TestCase(null, 7.05)]
        [TestCase(4.05, null)]
        [TestCase(4.05, 7.05)]
        [TestCase(null, null)]
        public async Task ShouldHaveUnknownWageForVacancyWithZeroHoursPerWeek(decimal? lowerBound, decimal? upperBound)
        {
            var minNationalWageLowerBound = lowerBound;
            var minNationalWageUpperBound = upperBound;

            _mockMediator.Setup(m => m.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetApprenticeshipVacancyResponse
                {
                    ApprenticeshipVacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)WageType.NationalMinimum)
                                            .With(v => v.MinimumWageLowerBound, minNationalWageLowerBound)
                                            .With(v => v.MinimumWageUpperBound, minNationalWageUpperBound)
                                            .With(v => v.HoursPerWeek, 0)
                                            .Without(v => v.WageUnitId)
                                            .Create()
                });

            var result = await _sut.GetApprenticeshipVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.WageUnit.Should().BeNull();
            result.WageText.Should().Be(UnknownwWageText);
        }
    }
}
