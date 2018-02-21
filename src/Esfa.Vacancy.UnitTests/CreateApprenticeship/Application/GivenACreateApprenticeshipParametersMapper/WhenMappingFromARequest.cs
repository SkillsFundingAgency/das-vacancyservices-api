using System;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;


namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingFromARequest
    {
        private EmployerInformation _employerInformation;
        private CreateApprenticeshipParameters _mappedParameters;
        private CreateApprenticeshipRequest _request;
        private int _randomWageType;
        private int _randomLegacyWageType;
        private DurationType _randomDurationType;
        private DomainDurationType _randomDomainDurationType;
        private Mock<IWageTypeMapper> _mockWageTypeMapper;
        private int _randomWageUnit;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _randomWageType = fixture.Create<int>();
            _randomLegacyWageType = fixture.Create<int>();
            _randomWageUnit = fixture.Create<int>();

            _randomDurationType = (DurationType)new Random().Next(1, 4);
            var durationTypeMapper = new DurationMapper();
            _randomDomainDurationType = durationTypeMapper.MapTypeToDomainType(_randomDurationType);

            _request = fixture.Build<CreateApprenticeshipRequest>()
                .With(request => request.WageType, (WageType)_randomWageType)
                .With(request => request.WageUnit, (WageUnit)_randomWageUnit)
                              .With(request => request.DurationType, _randomDurationType)
                              .Create();

            _mockWageTypeMapper = fixture.Freeze<Mock<IWageTypeMapper>>();
            _mockWageTypeMapper
                .Setup(typeMapper => typeMapper.MapToLegacy(It.IsAny<CreateApprenticeshipRequest>()))
                .Returns((LegacyWageType)_randomLegacyWageType);

            fixture.Inject<IDurationMapper>(durationTypeMapper);

            _employerInformation = fixture.Create<EmployerInformation>();

            var mapper = fixture.Create<CreateApprenticeshipParametersMapper>();

            _mappedParameters = mapper.MapFromRequest(_request, _employerInformation);
        }

        [Test]
        public void ThenMapsTitle()
        {
            _mappedParameters.Title.Should().Be(_request.Title);
        }

        [Test]
        public void ThenMapsShortDescription()
        {
            _mappedParameters.ShortDescription.Should().Be(_request.ShortDescription);
        }

        [Test]
        public void ThenMapsLongDescription()
        {
            _mappedParameters.Description.Should().Be(_request.LongDescription);
        }

        [Test]
        public void ThenMapsApplicationClosingDate()
        {
            _mappedParameters.ApplicationClosingDate.Should().Be(_request.ApplicationClosingDate);
        }

        [Test]
        public void ThenMapsExpectedStartDate()
        {
            _mappedParameters.ExpectedStartDate.Should().Be(_request.ExpectedStartDate);
        }

        [Test]
        public void ThenMapsDesiredSkills()
        {
            _mappedParameters.DesiredSkills.Should().Be(_request.DesiredSkills);
        }

        [Test]
        public void ThenMapsDesiredPersonalQualities()
        {
            _mappedParameters.DesiredPersonalQualities.Should().Be(_request.DesiredPersonalQualities);
        }

        [Test]
        public void ThenMapsDesiredQualifications()
        {
            _mappedParameters.DesiredQualifications.Should().Be(_request.DesiredQualifications);
        }

        [Test]
        public void ThenMapsFutureProspects()
        {
            _mappedParameters.FutureProspects.Should().Be(_request.FutureProspects);
        }

        [Test]
        public void ThenMapsThingsToConsider()
        {
            _mappedParameters.ThingsToConsider.Should().Be(_request.ThingsToConsider);
        }

        [Test]
        public void ThenMapsTrainingToBeProvided()
        {
            _mappedParameters.TrainingToBeProvided.Should().Be(_request.TrainingToBeProvided);
        }

        [Test]
        public void ThenAssignsValueFromDurationTypeMapper()
        {
            _mappedParameters.DurationTypeId.Should().Be((int)_randomDomainDurationType);
        }

        [Test]
        public void ThenMapsDurationValue()
        {
            _mappedParameters.DurationValue.Should().Be(_request.ExpectedDuration);
        }

        [Test]
        public void ThenMapsWorkingWeek()
        {
            _mappedParameters.WorkingWeek.Should().Be(_request.WorkingWeek);
        }

        [Test]
        public void ThenUsesWageTypeMapper()
        {
            _mockWageTypeMapper.Verify(
                mapper => mapper.MapToLegacy(
                    It.Is<CreateApprenticeshipRequest>(req => req.WageType == (WageType) _randomWageType)), 
                    Times.Once);
        }

        [Test]
        public void ThenAssignsValueFromWageTypeMapper()
        {
            _mappedParameters.WageType.Should().Be(_randomLegacyWageType);
        }

        [Test]
        public void ThenMapsWageTypeReason()
        {
            _mappedParameters.WageTypeReason.Should().Be(_request.WageTypeReason);
        }

        [Test]
        public void ThenMapsWageUnit()
        {
            _mappedParameters.WageUnitId.Should().Be(_randomWageUnit);
        }

        [Test]
        public void ThenMapsMinWage()
        {
            _mappedParameters.WageLowerBound.Should().Be(_request.MinWage);
        }

        [Test]
        public void ThenMapsMaxWage()
        {
            _mappedParameters.WageUpperBound.Should().Be(_request.MaxWage);
        }

        [Test]
        public void ThenMapsWeeklyWage()
        {
            _mappedParameters.WeeklyWage.Should().Be(_request.FixedWage);
        }

        [Test]
        public void ThenMapsNumberOfPositions()
        {
            _mappedParameters.NumberOfPositions.Should().Be(_request.NumberOfPositions);
        }

        [Test]
        public void ThenMapsVacancyOwnerRelationshipId()
        {
            _mappedParameters.VacancyOwnerRelationshipId
                .Should().Be(_employerInformation.VacancyOwnerRelationshipId);
        }

        [Test]
        public void ThenMapsProviderId()
        {
            _mappedParameters.ProviderId
                .Should().Be(_employerInformation.ProviderId);
        }

        [Test]
        public void ThenMapsProviderSiteId()
        {
            _mappedParameters.ProviderSiteId
                .Should().Be(_employerInformation.ProviderSiteId);
        }

        [Test]
        public void ThenMapsEmployerDescription()
        {
            _mappedParameters.EmployerDescription
                .Should().Be(_employerInformation.EmployerDescription);
        }

        [Test]
        public void ThenMapsEmployerWebsite()
        {
            _mappedParameters.EmployersWebsite
                .Should().Be(_employerInformation.EmployerWebsite);
        }

        [Test]
        public void ThenMapsContactName()
        {
            _mappedParameters.ContactName
                .Should().Be(_request.ContactName);
        }

        [Test]
        public void ThenMapsContactEmail()
        {
            _mappedParameters.ContactEmail
                .Should().Be(_request.ContactEmail);
        }

        [Test]
        public void ThenMapsContactNumber()
        {
            _mappedParameters.ContactNumber
                .Should().Be(_request.ContactNumber);
        }

        [Test]
        public void ThenMapsTrainingType()
        {
            _mappedParameters.TrainingType.Should().Be((int)_request.TrainingType);
        }
    }
}