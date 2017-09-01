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
            //Arrange
            var response = new GetVacancyResponse
            {
                Vacancy = new Domain.Entities.Vacancy
                {
                    WageType = (int)WageType.Custom,
                    WageUnitId = wageUnitId
                }
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var vacancy = await _sut.GetVacancyDetailsAsync(12345);
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
            result.Wage.Should().BeNullOrEmpty();
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
        public async Task ShouldHaveNullWageUnitForVacanciesWithNonCustomOrLegacyWeeklyWageType(WageType nonCustomWageType)
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)nonCustomWageType)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Apprenticeship);
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
        public async Task ShouldHaveCorrectWageUnitSetForVacanciesWithCustomOrCustomRangeOrLegacyWeeklyWageType(WageType nonCustomWageType, WageUnit wageUnit, WageUnit expectedWageUnit)
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)nonCustomWageType)
                                            .With(v => v.WageUnitId, (int)wageUnit)
                                            .Without(v => v.WageText)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Apprenticeship);
            result.WageUnit.Should().NotBeNull();
            result.WageUnit.Should().Be(expectedWageUnit);
        }
        
        [Test]
        public async Task ShouldHaveUnknownWageForVacanciesWithLegacyTextWageType()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)WageType.LegacyText)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Apprenticeship);
            result.WageUnit.Should().BeNull();
            result.Wage.Should().Be("Unknown");
        }

        [TestCase(WageType.Unwaged, "Unwaged")]
        [TestCase(WageType.ToBeAgreedUponAppointment, "To be agreed upon appointment")]
        [TestCase(WageType.CompetitiveSalary, "Competitive salary")]
        public async Task ShouldHaveAppropriateWageDescriptionForVacanciesWithNoWageAmount(WageType wageType, string expectedWageText)
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)wageType)
                                            .Without(v => v.WeeklyWage)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Apprenticeship);
            result.WageUnit.Should().BeNull();
            result.Wage.Should().Be(expectedWageText);
        }

        [Test]
        public async Task LiveVacanciesWithLegacyWeeklyWageTypeShouldHaveWageSetFromWeeklyWage()
        {
            const int weeklyWage = 2550;

            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)WageType.LegacyWeekly)
                                            .With(v => v.WeeklyWage, weeklyWage)
                                            .Without(v => v.WageText)
                                            .With(v => v.WageUnitId, (int)WageUnit.Weekly)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Apprenticeship);
            result.WageUnit.Should().Be(WageUnit.Weekly);
            result.Wage.Should().Be("£2,550");
        }

        [TestCase(WageUnit.Weekly)]
        [TestCase(WageUnit.Monthly)]
        [TestCase(WageUnit.Annually)]
        [TestCase(WageUnit.NotApplicable)]
        public async Task ShouldHaveWageSetForVacanciesWithCustomWageType(WageUnit wageUnit)
        {
            const int weeklyWage = 2550;

            _mockMediator.Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVacancyResponse
                {
                    Vacancy = new Fixture().Build<Domain.Entities.Vacancy>()
                                            .With(v => v.VacancyReferenceNumber, VacancyReference)
                                            .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                                            .With(v => v.VacancyTypeId, (int)VacancyType.Apprenticeship)
                                            .With(v => v.WageType, (int)WageType.Custom)
                                            .With(v => v.WeeklyWage, weeklyWage)
                                            .Without(v => v.WageText)
                                            .With(v => v.WageUnitId, (int)wageUnit)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.VacancyType.Should().Be(VacancyType.Apprenticeship);
            result.WageUnit.Should().Be(wageUnit);
            result.Wage.Should().Be("£2,550");
        }

        [TestCase(WageUnit.Weekly, 14000, 16000, "£14,000 - £16,000")]
        [TestCase(WageUnit.Weekly, null, 16000, "Unknown - £16,000")]
        [TestCase(WageUnit.Weekly, 14000, null, "£14,000 - Unknown")]
        [TestCase(WageUnit.Monthly, 14000, 16000, "£14,000 - £16,000")]
        [TestCase(WageUnit.Monthly, null, 16000, "Unknown - £16,000")]
        [TestCase(WageUnit.Monthly, 14000, null, "£14,000 - Unknown")]
        [TestCase(WageUnit.Annually, 14000, 16000, "£14,000 - £16,000")]
        [TestCase(WageUnit.Annually, null, 16000, "Unknown - £16,000")]
        [TestCase(WageUnit.Annually, 14000, null, "£14,000 - Unknown")]
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
            result.Wage.Should().Be(expectedWageText);
        }
    }
}
