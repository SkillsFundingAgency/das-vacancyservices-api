using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Threading.Tasks;
using ApprenticeshipVacancyDto = Esfa.Vacancy.Api.Types.ApprenticeshipVacancy;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
	 [TestFixture]
	 public class WhenMappingEmployersRecruitmentWebsite: RecruitApprenticeshipMapperBase
	 {
		  [SetUp]
		  public void Setup()
		  {
				Initialize();
		  }

		  [Test]
		  public async Task ShouldSetApplicationUrl()
		  {
				RecruitApprenticeshipMapper sut = GetRecruitApprecticeshipMapper();
				string expectedUrl = "https://" + Guid.NewGuid();
				LiveVacancy.ApplicationUrl = expectedUrl;
				ApprenticeshipVacancyDto result = await sut.MapFromRecruitVacancy(LiveVacancy);

				result.ApplicationUrl.Should().Be(expectedUrl);
		  }

	 }
}