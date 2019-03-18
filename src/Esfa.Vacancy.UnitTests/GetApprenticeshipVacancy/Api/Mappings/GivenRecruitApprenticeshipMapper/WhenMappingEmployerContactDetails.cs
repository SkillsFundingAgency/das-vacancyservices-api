using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingEmployerContactDetails : RecruitApprenticeshipMapperBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
        }

        [Test]
        public async Task ThenMapFilledEmployerContactDetails()
        {
            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.EmployerContactName = "NormanNotEmpty";
            LiveVacancy.EmployerContactEmail = "NormanNotEmpty@needanemail.com";
            LiveVacancy.EmployerContactPhone = "0800464646";

            var mappedVacancy = await sut.MapFromRecruitVacancy(LiveVacancy);
            
            mappedVacancy.ContactName.ShouldBeEquivalentTo("NormanNotEmpty");
            mappedVacancy.ContactEmail.ShouldBeEquivalentTo("NormanNotEmpty@needanemail.com");
            mappedVacancy.ContactNumber.ShouldBeEquivalentTo("0800464646");
        }

        [Test]
        public async Task ThenMapUnFilledEmployerContactDetails()
        {
            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.EmployerContactName = null;
            LiveVacancy.EmployerContactEmail = string.Empty;
            LiveVacancy.EmployerContactPhone = null;

            var mappedVacancy = await sut.MapFromRecruitVacancy(LiveVacancy);

            mappedVacancy.ContactName.Should().BeNullOrEmpty();
            mappedVacancy.ContactEmail.Should().BeNullOrEmpty();
            mappedVacancy.ContactNumber.Should().BeNullOrEmpty();
        }
    }
}
