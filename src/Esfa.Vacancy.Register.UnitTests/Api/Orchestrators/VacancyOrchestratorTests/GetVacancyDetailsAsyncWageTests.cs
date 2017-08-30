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
    [TestFixture()]
    public class GetVacancyDetailsAsyncWageTests
    {

        private const int VacancyReference = 1234;
        private const int LiveVacancyStatusId = 2;
        private Mock<IMediator> _mockMediator;
        private Mock<IProvideSettings> _provideSettings;
        private VacancyOrchestrator _sut;

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
            _sut = new VacancyOrchestrator(_mockMediator.Object, _provideSettings.Object);
        }

        [TestCase(1, WageUnit.NotApplicable)]
        [TestCase(2, WageUnit.Weekly)]
        [TestCase(3, WageUnit.Monthly)]
        [TestCase(4, WageUnit.Annually)]
        [TestCase(null, null)]
        public async Task MappingWageUnitTests(int? wageUnitId, WageUnit? wageUnitType)
        {
            var provideSettingsMock = new Mock<IProvideSettings>();
            var mediatorMock = new Mock<IMediator>();
            var response = new GetVacancyResponse()
            {
                Vacancy = new Domain.Entities.Vacancy() { WageType = (int)WageType.Custom, WageUnitId = wageUnitId }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var sut = new VacancyOrchestrator(mediatorMock.Object, provideSettingsMock.Object);
            //Act
            var vacancy = await sut.GetVacancyDetailsAsync(12345);
            //Assert
            vacancy.WageUnit.ShouldBeEquivalentTo(wageUnitType);
        }

        [Test]
        public async Task ShouldNotHaveWageFieldsSetForTraineeships()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Traineeship)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Traineeship);
            result.WageText.Should().BeNullOrEmpty();
            result.WageUnit.Should().BeNull();
            result.WorkingWeek.Should().NotBeNullOrEmpty();
            result.HoursPerWeek.Should().BeNull();
        }

        [TestCase(WageType.ApprenticeshipMinimum)]
        [TestCase(WageType.CompetitiveSalary)]
        [TestCase(WageType.LegacyText)]
        [TestCase(WageType.NationalMinimum)]
        [TestCase(WageType.ToBeAgreedUponAppointment)]
        [TestCase(WageType.Unwaged)]
        public async Task ShouldHaveWageFieldsSetForApprenticeshipsWithNonCustomOrLegacyWeeklyWageType(WageType nonCustomWageType)
        {
            const int weeklyWage = 2550;

            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WeeklyWage, weeklyWage)
                                            .With(v => v.WageType, (int)nonCustomWageType)
                                            .With(v => v.WageUnitId, 0)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Apprenticeship);

            result.WageText.Should().Be("£2,550");
            result.WageUnit.Should().BeNull();
        }

        [TestCase(WageType.Custom, WageUnit.Weekly, WageUnit.Weekly)]
        [TestCase(WageType.Custom, WageUnit.Monthly, WageUnit.Monthly)]
        [TestCase(WageType.Custom, WageUnit.Annually, WageUnit.Annually)]
        [TestCase(WageType.CustomRange, WageUnit.Weekly, WageUnit.Weekly)]
        [TestCase(WageType.CustomRange, WageUnit.Monthly, WageUnit.Monthly)]
        [TestCase(WageType.CustomRange, WageUnit.Annually, WageUnit.Annually)]
        [TestCase(WageType.CustomRange, WageUnit.NotApplicable, WageUnit.Weekly)]
        [TestCase(WageType.LegacyWeekly, WageUnit.Weekly, WageUnit.Weekly)]
        [TestCase(WageType.LegacyWeekly, WageUnit.Monthly, WageUnit.Weekly)]
        [TestCase(WageType.LegacyWeekly, WageUnit.Annually, WageUnit.Weekly)]
        [TestCase(WageType.LegacyWeekly, WageUnit.NotApplicable, WageUnit.Weekly)]
        public async Task ShouldHaveWageFieldsSetForApprenticeshipsWithCustomOrCustomRangeOrLegacyWeeklyWageType(WageType nonCustomWageType, WageUnit wageUnit, WageUnit expectedWageUnit)
        {
            const int weeklyWage = 2550;

            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WeeklyWage, weeklyWage)
                                            .With(v => v.WageType, (int)nonCustomWageType)
                                            .With(v => v.WageUnitId)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Apprenticeship);

            result.WageText.Should().Be("£2,550");
            result.WageUnit.Should().NotBeNull();
        }
        
        [TestCase(0)]
        [TestCase(null)]
        public async Task ShouldHaveWageFieldsSetAsUnknownForApprenticeshipWithNoWage(int wage)
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WeeklyWage, wage)
                                            .With(v => v.WageType, (int)WageType.Custom)
                                            .Without(v => v.WageUnitId)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Apprenticeship);

            result.WageText.Should().Be("Unknown");
        }
    }
}
