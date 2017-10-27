﻿using AutoMapper;
using Esfa.Vacancy.Register.Api;
using FluentAssertions;
using NUnit.Framework;
using ApiTypes = Esfa.Vacancy.Api.Types;
using DomainTypes = Esfa.Vacancy.Register.Domain.Entities;


namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.Mappings
{
    public class GivenApprenticeshipSummaryMapper
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [TestCase(1, null, ApiTypes.TrainingType.Standard, TestName = "Then load Standard type")]
        [TestCase(null, "10", ApiTypes.TrainingType.Framework, TestName = "Then load Framework type")]
        public void ThenLoadCorrectTraingingDetails(int? standardId, string frameworkCode, ApiTypes.TrainingType expectedTrainingType)
        {
            var expectedTrainingCode = standardId.HasValue ? standardId.ToString() : frameworkCode;
            var domainType = new DomainTypes.ApprenticeshipSummary()
            {
                FrameworkCode = frameworkCode,
                StandardId = standardId
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.TrainingType.Should().Be(expectedTrainingType);
            result.TrainingCode.Should().Be(expectedTrainingCode);
        }

        [Test]
        public void ThenLoadCorrectLocationGeoCoordinates()
        {
            var domainType = new DomainTypes.ApprenticeshipSummary
            {
                Location = new DomainTypes.GeoPoint() { Lat = 12.1213, Lon = 34.2343424 }
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.Location.Latitude.Should().Be(12.1213m);
            result.Location.Longitude.Should().Be(34.2343424m);

        }
    }
}
