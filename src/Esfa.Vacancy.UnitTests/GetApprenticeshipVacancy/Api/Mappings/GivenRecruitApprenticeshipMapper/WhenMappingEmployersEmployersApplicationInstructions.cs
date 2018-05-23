using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using ApprenticeshipVacancyDto = Esfa.Vacancy.Api.Types.ApprenticeshipVacancy;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
	 [TestFixture]
	 public class WhenMappingEmployersApplicationInstructions: RecruitApprenticeshipMapperBase
	 {
		  [SetUp]
		  public void Setup()
		  {
				Initialize();
		  }

		  [Test]
		  public async void ShouldSetApplicationInstructions()
		  {
				RecruitApprenticeshipMapper sut = GetRecruitApprecticeshipMapper();
				string expectedInstructions = Guid.NewGuid().ToString();

				LiveVacancy.ApplicationInstructions = expectedInstructions;

				ApprenticeshipVacancyDto result = await sut.MapFromRecruitVacancy(LiveVacancy);

				result.ApplicationInstructions.Should().Be(expectedInstructions);
		  }

	 }
}