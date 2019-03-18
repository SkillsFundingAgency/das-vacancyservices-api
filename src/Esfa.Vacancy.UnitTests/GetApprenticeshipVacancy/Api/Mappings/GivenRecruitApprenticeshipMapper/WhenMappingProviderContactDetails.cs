using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingProviderContactDetails : RecruitApprenticeshipMapperBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
        }

        [Test]
        public async Task ThenMapFilledProviderContactDetails()
        {
            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.ProviderContactName = "NormanNotEmpty";
            LiveVacancy.ProviderContactEmail = "NormanNotEmpty@needanemail.com";
            LiveVacancy.ProviderContactPhone = "0800464646";

            var mappedVacancy = await sut.MapFromRecruitVacancy(LiveVacancy);

            LiveVacancy.EmployerContactName.Should().BeNullOrEmpty();
            LiveVacancy.EmployerContactEmail.Should().BeNullOrEmpty();
            LiveVacancy.EmployerContactEmail.Should().BeNullOrEmpty();

            mappedVacancy.ContactName.ShouldBeEquivalentTo("NormanNotEmpty");
            mappedVacancy.ContactEmail.ShouldBeEquivalentTo("NormanNotEmpty@needanemail.com");
            mappedVacancy.ContactNumber.ShouldBeEquivalentTo("0800464646");
        }

        [Test]
        public async Task ThenMapUnFilledProviderContactDetails()
        {
            var sut = GetRecruitApprecticeshipMapper();
            LiveVacancy.ProviderContactName = null;
            LiveVacancy.ProviderContactEmail = string.Empty;
            LiveVacancy.ProviderContactPhone = null;

            var mappedVacancy = await sut.MapFromRecruitVacancy(LiveVacancy);

            LiveVacancy.EmployerContactName.Should().BeNullOrEmpty();
            LiveVacancy.EmployerContactEmail.Should().BeNullOrEmpty();
            LiveVacancy.EmployerContactEmail.Should().BeNullOrEmpty();

            mappedVacancy.ContactName.Should().BeNullOrEmpty();
            mappedVacancy.ContactEmail.Should().BeNullOrEmpty();
            mappedVacancy.ContactNumber.Should().BeNullOrEmpty();
        }
    }
}
