using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using DomainTypes = Esfa.Vacancy.Register.Domain.Entities;


namespace Esfa.Vacancy.Register.UnitTests.GetTraineeshipVacancy.Api.Mappings
{
    [TestFixture]
    public class GivenTraineeshipMapper
    {
        private TraineeshipMapper _sut;
        private readonly Mock<IProvideSettings> _provideSettingsMock = new Mock<IProvideSettings>();

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new TraineeshipMapper(_provideSettingsMock.Object);
        }

        [TestCase(1, false, TestName = "And LocationType is 1 Then set IsNationwide to false")]
        [TestCase(2, false, TestName = "And LocationType is 2 Then set IsNationwide to false")]
        [TestCase(3, true, TestName = "And LocationType is 3 Then set IsNationwide to true")]
        public void WhenMappingIsNationwide(int vacancyLocationTypeid, bool expectedResult)
        {
            var vacancy = new DomainTypes.TraineeshipVacancy()
            {
                VacancyLocationTypeId = vacancyLocationTypeid,
                Location = new DomainTypes.Address()
            };

            var result = _sut.MapToTraineeshipVacancy(vacancy);

            result.IsNationwide.Should().Be(expectedResult);
        }

        [TestCase("", TestName = "And Anonymous Employer Name is empty Then populate using EmployerName")]
        [TestCase("Anonymous name", TestName = "And Anonymous Employer Name is given Then populate using AnonymousEmployerName")]
        public void WhenMappingEmployerName(string anonymousEmployerName)
        {
            var vacancy = new DomainTypes.TraineeshipVacancy()
            {
                EmployerName = "Employer Name",
                AnonymousEmployerName = anonymousEmployerName,
                Location = new DomainTypes.Address()
            };

            var expectedEmployerName =
                string.IsNullOrWhiteSpace(vacancy.AnonymousEmployerName) ? vacancy.EmployerName : vacancy.AnonymousEmployerName;

            var result = _sut.MapToTraineeshipVacancy(vacancy);

            result.EmployerName.Should().Be(expectedEmployerName);
        }

        [TestCase("", TestName = "And Anonymous Employer Name is empty Then populate using EmployerDescription")]
        [TestCase("Anonymous desc", TestName = "And Anonymous Employer Name is given Then populate using AnonymousEmployerDescription")]
        public void WhenMappingEmployerDescription(string anonymousEmployerName)
        {
            var vacancy = new DomainTypes.TraineeshipVacancy()
            {
                AnonymousEmployerDescription = "Anonymous Employer Desc",
                EmployerDescription = "Employer Desc",
                AnonymousEmployerName = anonymousEmployerName,
                Location = new DomainTypes.Address()
            };

            var expectedEmployerDescription =
                string.IsNullOrWhiteSpace(vacancy.AnonymousEmployerName) ? vacancy.EmployerDescription : vacancy.AnonymousEmployerDescription;

            var result = _sut.MapToTraineeshipVacancy(vacancy);

            result.EmployerDescription.Should().Be(expectedEmployerDescription);
        }

        [TestCase("", TestName = "And Anonymous Employer Name is empty Then populate using EmployerDescription")]
        [TestCase("Anonymous desc", TestName = "And Anonymous Employer Name is given Then populate using AnonymousEmployerDescription")]
        public void WhenMappingEmployerWebsite(string anonymousEmployerName)
        {
            var vacancy = new DomainTypes.TraineeshipVacancy()
            {
                EmployerWebsite = "www.google.com",
                AnonymousEmployerName = anonymousEmployerName,
                Location = new DomainTypes.Address()
            };

            var expectedEmployerWebsite =
                string.IsNullOrWhiteSpace(vacancy.AnonymousEmployerName) ? vacancy.EmployerWebsite : null;

            var result = _sut.MapToTraineeshipVacancy(vacancy);

            result.EmployerWebsite.Should().Be(expectedEmployerWebsite);
        }
    }
}
