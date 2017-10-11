using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api;
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
        private const int VacancyReference = 1234;
        private const int LiveVacancyStatusId = 2;
        private Mock<IMediator> _mockMediator;
        private Mock<IProvideSettings> _provideSettings;
        private GetVacancyOrchestrator _sut;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _provideSettings = new Mock<IProvideSettings>();
            _sut = new GetVacancyOrchestrator(_mockMediator.Object, _provideSettings.Object, new Mapper(AutoMapperConfig.Configure()));
        }

        [TestCase(WageType.Unwaged, "Unwaged")]
        [TestCase(WageType.ToBeAgreedUponAppointment, "To be agreed upon appointment")]
        [TestCase(WageType.CompetitiveSalary, "Competitive salary")]
        public async Task ShouldHaveAppropriateWageDescription(WageType wageType, string expectedWageText)
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
                                            .Without(v => v.WageUnitId)
                                            .Create()
                });

            var result = await _sut.GetVacancyDetailsAsync(VacancyReference);

            result.VacancyReference.Should().Be(VacancyReference);
            result.WageUnit.Should().BeNull();
            result.WageText.Should().Be(expectedWageText);
        }
    }
}
