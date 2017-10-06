using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.GetVacancy.Api.Orchestrators.VacancyOrchestratorTests
{
    [TestFixture]
    public class GetVacancyDetailsAsyncVacancyUrlTests
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [Test]
        public async Task WhenBaseUrlIsDefined_ShouldPopulateVacancyUrl()
        {
            //Arrange
            var baseUrl = "https://findapprentice.com/apprenticeship/reference";
            var vacancyReferenceNumber = 123456;
            var provideSettingsMock = new Mock<IProvideSettings>();
            provideSettingsMock.Setup(p => p.GetSetting(It.IsAny<string>())).Returns(baseUrl);

            var mediatorMock = new Mock<IMediator>();
            var response = new GetVacancyResponse()
            {
                Vacancy = new Domain.Entities.Vacancy() { VacancyReferenceNumber = vacancyReferenceNumber }
            };
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var sut = new GetVacancyOrchestrator(mediatorMock.Object, provideSettingsMock.Object, _mapper);
            //Act
            var vacancy = await sut.GetVacancyDetailsAsync(12345);

            //Assert
            Assert.AreEqual($"{baseUrl}/{vacancyReferenceNumber}", vacancy.VacancyUrl);
        }

        [TestCase(1, VacancyType.Apprenticeship)]
        [TestCase(2, VacancyType.Traineeship)]
        [TestCase(null, VacancyType.Apprenticeship)]
        public async Task MappingVacancyTypeTests(int? vacancyTypeId, VacancyType vacancyType)
        {
            var provideSettingsMock = new Mock<IProvideSettings>();
            var mediatorMock = new Mock<IMediator>();
            var response = new GetVacancyResponse()
            {
                Vacancy = new Domain.Entities.Vacancy() { VacancyTypeId = vacancyTypeId }
            };
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var sut = new GetVacancyOrchestrator(mediatorMock.Object, provideSettingsMock.Object, _mapper);
            //Act
            var vacancy = await sut.GetVacancyDetailsAsync(12345);
            //Assert
            vacancy.VacancyType.ShouldBeEquivalentTo(vacancyType);
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
                Vacancy = new Domain.Entities.Vacancy() { WageUnitId = wageUnitId }
            };
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var sut = new GetVacancyOrchestrator(mediatorMock.Object, provideSettingsMock.Object, _mapper);
            //Act
            var vacancy = await sut.GetVacancyDetailsAsync(12345);
            //Assert
            vacancy.WageUnit.ShouldBeEquivalentTo(wageUnitType);
        }
    }
}
