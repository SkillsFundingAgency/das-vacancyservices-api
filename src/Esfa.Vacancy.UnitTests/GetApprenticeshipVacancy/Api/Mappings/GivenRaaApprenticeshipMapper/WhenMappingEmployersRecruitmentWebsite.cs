﻿using Esfa.Vacancy.Domain.Entities;
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
    public class WhenMappingEmployersRecruitmentWebsite
    {
        private ApprenticeshipMapper _sut;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            var provideSettings = new Mock<IProvideSettings>();
            _sut = new ApprenticeshipMapper(provideSettings.Object);
        }

        [Test]
        public void ThenSetApplicationUrl()
        {
            string expectedUrl = "https://" + Guid.NewGuid();
            var vacancy = new ApprenticeshipVacancy
            {
                EmployersRecruitmentWebsite = expectedUrl,
                Location = new Address(),
                ApprenticeshipTypeId = 1
            };

            ApprenticeshipVacancyDto result = _sut.MapToApprenticeshipVacancy(vacancy);

            result.ApplicationUrl.Should().Be(expectedUrl);
        }

    }
}