using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipParametersMapper.WhenMappingLocation
{
    [TestFixture]
    public class AndLocationTypeIsOtherLocation
    {
        private CreateApprenticeshipParameters _mappedParameters;
        private EmployerInformation _employerInformation;
        private CreateApprenticeshipRequest _createApprenticeshipRequest;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _createApprenticeshipRequest = fixture.Build<CreateApprenticeshipRequest>()
                .With(request => request.LocationType, LocationType.OtherLocation)
                .Create();
            _employerInformation = fixture.Create<EmployerInformation>();

            var mapper = fixture.Create<CreateApprenticeshipParametersMapper>();
            _mappedParameters = mapper.MapFromRequest(_createApprenticeshipRequest, _employerInformation);
        }

        [Test]
        public void ThenMapLocationTypeToNationwide()
        {
            _mappedParameters.LocationTypeId.Should().Be(1);
        }

        [Test]
        public void ThenMapAddressLine1FromEmployeeInformation()
        {
            _mappedParameters.AddressLine1.Should().Be(_createApprenticeshipRequest.AddressLine1);
            _mappedParameters.AddressLine1.Should().NotBe(_employerInformation.AddressLine1);
        }

        [Test]
        public void ThenMapAddressLine2FromEmployeeInformation()
        {
            _mappedParameters.AddressLine2.Should().Be(_createApprenticeshipRequest.AddressLine2);
            _mappedParameters.AddressLine2.Should().NotBe(_employerInformation.AddressLine2);
        }

        [Test]
        public void ThenMapAddressLine3FromEmployeeInformation()
        {
            _mappedParameters.AddressLine3.Should().Be(_createApprenticeshipRequest.AddressLine3);
            _mappedParameters.AddressLine3.Should().NotBe(_employerInformation.AddressLine3);
        }

        [Test]
        public void ThenMapAddressLine5FromEmployeeInformation()
        {
            _mappedParameters.AddressLine5.Should().Be(_createApprenticeshipRequest.AddressLine5);
            _mappedParameters.AddressLine5.Should().NotBe(_employerInformation.AddressLine5);
        }

        [Test]
        public void ThenMapTownFromEmployeeInformation()
        {
            _mappedParameters.Town.Should().Be(_createApprenticeshipRequest.Town);
            _mappedParameters.Town.Should().NotBe(_employerInformation.Town);
        }

        [Test]
        public void ThenMapPostcodeFromEmployeeInformation()
        {
            _mappedParameters.Postcode.Should().Be(_createApprenticeshipRequest.Postcode);
            _mappedParameters.Postcode.Should().NotBe(_employerInformation.Postcode);
        }

        [Test]
        public void ThenMapAddressLine4FromEmployeeInformation()
        {
            _mappedParameters.AddressLine4.Should().Be(_createApprenticeshipRequest.AddressLine4);
            _mappedParameters.AddressLine4.Should().NotBe(_employerInformation.AddressLine4);
        }
    }
}
