﻿using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Register.Api.Mappings;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRaaApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingIsNationwide
    {
        private ApprenticeshipMapper _sut;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            var provideSettings = new Mock<IProvideSettings>();
            _sut = new ApprenticeshipMapper(provideSettings.Object);
        }

        [TestCase(1, false, TestName = "And LocationTypeId is 1 Then set IsNationwide to false")]
        [TestCase(2, false, TestName = "And LocationTypeId is 2 Then set IsNationwide to false")]
        [TestCase(3, true, TestName = "And LocationTypeId is 3 Then set IsNationwide to true")]
        public void ShouldMapUsingLocationTypeId(int vacancyLocationTypeid, bool expectedResult)
        {
            var vacancy = new ApprenticeshipVacancy()
            {
                VacancyLocationTypeId = vacancyLocationTypeid,
                Location = new Address(),
                ApprenticeshipTypeId = 1
            };

            var result = _sut.MapToApprenticeshipVacancy(vacancy);

            result.IsNationwide.Should().Be(expectedResult);
        }
    }
}
