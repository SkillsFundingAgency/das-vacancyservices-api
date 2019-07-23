using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRaaApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingEmployerInformation
    {
        private ApprenticeshipMapper _sut;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            var provideSettings = new Mock<IProvideSettings>();
            _sut = new ApprenticeshipMapper(provideSettings.Object);
        }

        [Test]
        public void AndEmployerIsNotAnonymous_ThenDoNotReplaceEmployerNameAndDescription()
        {
            var vacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                .With(v => v.ApprenticeshipTypeId, 1)
                .With(v => v.WageUnitId, null)
                .With(v => v.VacancyReferenceNumber, 1234)
                .With(v => v.VacancyStatusId, 2)
                .With(v => v.EmployerName, "ABC Ltd")
                .With(v => v.EmployerDescription, "A plain company")
                .With(v => v.EmployerWebsite, "http://www.google.co.uk")
                .Without(v => v.AnonymousEmployerName)
                .Without(v => v.AnonymousEmployerDescription)
                .Without(v => v.AnonymousEmployerReason)
                .Create();

            var result = _sut.MapToApprenticeshipVacancy(vacancy);

            result.VacancyReference.Should().Be(vacancy.VacancyReferenceNumber);
            result.EmployerName.Should().Be("ABC Ltd");
            result.EmployerDescription.Should().Be("A plain company");
            result.EmployerWebsite.Should().Be("http://www.google.co.uk");
            result.Location.Should().NotBeNull();
            result.Location.AddressLine1.Should().NotBeNull();
            result.Location.AddressLine2.Should().NotBeNull();
            result.Location.AddressLine3.Should().NotBeNull();
            result.Location.AddressLine4.Should().NotBeNull();
            result.Location.AddressLine5.Should().NotBeNull();
            result.Location.Town.Should().NotBeNull();
            result.Location.PostCode.Should().NotBeNull();
            result.Location.GeoPoint.Should().NotBeNull();
            result.Location.GeoPoint.Longitude.Should().NotBeNull();
            result.Location.GeoPoint.Latitude.Should().NotBeNull();
        }

        [Test]
        public void AndEmployerIsAnonymous_ThenReplaceEmployerNameAndDescription()
        {
            var vacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                .With(v => v.ApprenticeshipTypeId, 1)
                .With(v => v.WageUnitId, null)
                .With(v => v.VacancyReferenceNumber, 1234)
                .With(v => v.VacancyStatusId, 2)
                .With(v => v.EmployerName, "Her Majesties Secret Service")
                .With(v => v.EmployerDescription, "A private description")
                .With(v => v.AnonymousEmployerName, "ABC Ltd")
                .With(v => v.AnonymousEmployerDescription, "A plain company")
                .With(v => v.AnonymousEmployerReason, "Because I want to test")
                .Create();

            var result = _sut.MapToApprenticeshipVacancy(vacancy);

            result.VacancyReference.Should().Be(vacancy.VacancyReferenceNumber);
            result.EmployerName.Should().Be("ABC Ltd");
            result.EmployerDescription.Should().Be("A plain company");
            result.EmployerWebsite.Should().BeNullOrEmpty();
            result.Location.AddressLine1.Should().BeNull();
            result.Location.AddressLine2.Should().BeNull();
            result.Location.AddressLine3.Should().BeNull();
            result.Location.AddressLine4.Should().BeNull();
            result.Location.AddressLine5.Should().BeNull();
            result.Location.PostCode.Should().BeNull();
            result.Location.GeoPoint.Latitude.Should().BeNull();
            result.Location.GeoPoint.Longitude.Should().BeNull();
            result.Location.Town.Should().NotBeNullOrWhiteSpace();

        }
    }
}