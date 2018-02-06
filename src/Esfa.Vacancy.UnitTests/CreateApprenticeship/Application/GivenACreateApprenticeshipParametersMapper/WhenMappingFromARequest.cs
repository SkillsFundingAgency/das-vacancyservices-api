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
        private Mock<IWageTypeMapper> _mockWageTypeMapper;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _randomWageType = fixture.Create<int>();
            _randomLegacyWageType = fixture.Create<int>();

            _request = fixture.Build<CreateApprenticeshipRequest>()
                .With(request => request.WageType, (WageType)_randomWageType)
                .Create();

            _mockWageTypeMapper = fixture.Freeze<Mock<IWageTypeMapper>>();
            _mockWageTypeMapper
                .Setup(typeMapper => typeMapper.MapToLegacy(It.IsAny<WageType>()))
                .Returns((LegacyWageType)_randomLegacyWageType);

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
        public void ThenMapsWorkingWeek()
        {
            _mappedParameters.WorkingWeek.Should().Be(_request.WorkingWeek);
        }

        [Test]
        public void ThenUsesWageTypeMapper()
        {
            _mockWageTypeMapper.Verify(mapper => mapper.MapToLegacy((WageType)_randomWageType), Times.Once);
        }

        [Test]
        public void ThenAssignsValueFromWageTypeMapper()
        {
            _mappedParameters.WageType.Should().Be((LegacyWageType)_randomLegacyWageType);
        }

        [Test]
        public void ThenMapsWageTypeReason()
        {
            _mappedParameters.WageTypeReason.Should().Be(_request.WageTypeReason);
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
    }
}