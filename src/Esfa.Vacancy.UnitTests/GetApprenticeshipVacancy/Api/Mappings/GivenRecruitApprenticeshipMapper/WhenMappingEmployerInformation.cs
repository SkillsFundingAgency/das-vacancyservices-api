using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Vacancies.Client.Entities;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingEmployerInformation : RecruitApprenticeshipMapperBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
        }

        [Test]
        public async Task AndEmployerIsNotAnonymous_ThenMapAsNotAnonymous()
        {
            SetEmployerInformation();

            LiveVacancy.IsAnonymous = false;

            var sut = GetRecruitApprecticeshipMapper();
            
            var mappedVacancy = await sut.MapFromRecruitVacancy(LiveVacancy);

            mappedVacancy.EmployerName.Should().Be("employer name");
            mappedVacancy.EmployerDescription.Should().Be("employer description");
            mappedVacancy.EmployerWebsite.Should().Be("http://employerwebsite.com");
            mappedVacancy.Location.AddressLine1.Should().Be("address line 1");
            mappedVacancy.Location.AddressLine2.Should().Be("address line 2");
            mappedVacancy.Location.AddressLine3.Should().Be("address line 3");
            mappedVacancy.Location.AddressLine4.Should().Be("address line 4");
            mappedVacancy.Location.PostCode.Should().Be("post code");
            mappedVacancy.Location.GeoPoint.Latitude.Should().Be(1.2m);
            mappedVacancy.Location.GeoPoint.Longitude.Should().Be(3.4m);
        }

        [Test]
        public async Task AndEmployerIsAnonymous_ThenMapAsAnonymous()
        {
            SetEmployerInformation();

            LiveVacancy.IsAnonymous = true;

            var sut = GetRecruitApprecticeshipMapper();

            var mappedVacancy = await sut.MapFromRecruitVacancy(LiveVacancy);

            mappedVacancy.EmployerName.Should().Be("employer name");
            mappedVacancy.EmployerDescription.Should().Be("employer description");
            mappedVacancy.EmployerWebsite.Should().BeNull();
            mappedVacancy.Location.AddressLine1.Should().BeNull();
            mappedVacancy.Location.AddressLine2.Should().BeNull();
            mappedVacancy.Location.AddressLine3.Should().BeNull();
            mappedVacancy.Location.AddressLine4.Should().BeNull();
            mappedVacancy.Location.PostCode.Should().Be("post code");
            mappedVacancy.Location.GeoPoint.Latitude.Should().BeNull();
            mappedVacancy.Location.GeoPoint.Longitude.Should().BeNull();
        }

        private void SetEmployerInformation()
        {
            LiveVacancy.EmployerName = "employer name";
            LiveVacancy.EmployerDescription = "employer description";
            LiveVacancy.EmployerWebsiteUrl = "http://employerwebsite.com";
            LiveVacancy.EmployerLocation = new Address
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                AddressLine3 = "address line 3",
                AddressLine4 = "address line 4",
                Postcode = "post code",
                Latitude = 1.2d,
                Longitude = 3.4d
            };
        }
    }
}
