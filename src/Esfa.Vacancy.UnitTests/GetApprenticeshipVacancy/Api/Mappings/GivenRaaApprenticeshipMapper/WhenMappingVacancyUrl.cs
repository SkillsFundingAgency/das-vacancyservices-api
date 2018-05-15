using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Register.Api.Mappings;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRaaApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingVacancyUrl
    {
        [Test]
        public void ShouldPopulateVacancyUrl()
        {
            //Arrange
            var provideSettings = new Mock<IProvideSettings>();
            var baseUrl = "https://findapprentice.com/apprenticeship/reference";
            provideSettings
                .Setup(p => p.GetSetting(ApplicationSettingKeys.LiveApprenticeshipVacancyBaseUrlKey))
                .Returns(baseUrl);

            var sut = new ApprenticeshipMapper(provideSettings.Object);

            var vacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                .With(v => v.WageUnitId, null)
                .Create();

            //Act
            var result = sut.MapToApprenticeshipVacancy(vacancy);

            //Assert
            Assert.AreEqual($"{baseUrl}/{vacancy.VacancyReferenceNumber}", result.VacancyUrl);
        }
    }
}