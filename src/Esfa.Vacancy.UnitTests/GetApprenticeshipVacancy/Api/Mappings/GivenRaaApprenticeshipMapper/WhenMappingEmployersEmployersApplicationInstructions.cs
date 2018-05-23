using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using ApprenticeshipVacancyDto = Esfa.Vacancy.Api.Types.ApprenticeshipVacancy;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRaaApprenticeshipMapper
{
	 [TestFixture]
	 public class WhenMappingEmployersApplicationInstructions
	 {
		  private ApprenticeshipMapper _sut;

		  [OneTimeSetUp]
		  public void FixtureSetup()
		  {
				var provideSettings = new Mock<IProvideSettings>();
				_sut = new ApprenticeshipMapper(provideSettings.Object);
		  }

		  [Test]
		  public void ShouldSetApplicationInstructions()
		  {
				string expectedInstructions = Guid.NewGuid().ToString();
				var vacancy = new ApprenticeshipVacancy
				{
					 EmployersApplicationInstructions = expectedInstructions,
					 Location = new Address()
				};

				ApprenticeshipVacancyDto result = _sut.MapToApprenticeshipVacancy(vacancy);

				result.ApplicationInstructions.Should().Be(expectedInstructions);
		  }

	 }
}