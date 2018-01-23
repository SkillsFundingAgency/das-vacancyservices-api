using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Manage.Api.Mappings;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ApplicationTypes = Esfa.Vacancy.Application.Commands.CreateApprenticeship;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Api.Mappings.GivenACreateApprenticeshipRequestMapper
{
    [TestFixture]
    public class WhenMappingFromApiParameters
    {
        private CreateApprenticeshipParameters _apiParameters;
        private CreateApprenticeshipRequest _mappedRequest;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();
            _apiParameters = fixture.Create<CreateApprenticeshipParameters>();

            var mapper = new CreateApprenticeshipRequestMapper();

            _mappedRequest = mapper.MapFromApiParameters(_apiParameters);
        }

        [Test]
        public void ThenMapsTitle()
        {
            _mappedRequest.Title.Should().Be(_apiParameters.Title);
        }

        [Test]
        public void ThenMapsShortDescription()
        {
            _mappedRequest.ShortDescription.Should().Be(_apiParameters.ShortDescription);
        }

        [Test]
        public void ThenMapsLongDescription()
        {
            _mappedRequest.LongDescription.Should().Be(_apiParameters.LongDescription);
        }

        [Test]
        public void ThenMapsApplicationClosingDate()
        {
            _mappedRequest.ApplicationClosingDate.Should().Be(_apiParameters.ApplicationClosingDate);
        }

        [Test]
        public void ThenMapsExpectedStartDate()
        {
            _mappedRequest.ExpectedStartDate.Should().Be(_apiParameters.ExpectedStartDate);
        }

        [Test]
        public void ThenMapsWorkingWeek()
        {
            _mappedRequest.WorkingWeek.Should().Be(_apiParameters.WorkingWeek);
        }

        [Test]
        public void ThenMapsHoursPerWeek()
        {
            _mappedRequest.HoursPerWeek.Should().Be(_apiParameters.HoursPerWeek);
        }

        [Test]
        public void ThenMapsLocationType()
        {
            _mappedRequest.LocationType.Should().Be((ApplicationTypes.LocationType)(int)_apiParameters.LocationType);
        }

        [Test]
        public void ThenMapsAddressLine1()
        {
            _mappedRequest.AddressLine1.Should().Be(_apiParameters.Location.AddressLine1);
        }

        [Test]
        public void ThenMapsAddressLine2()
        {
            _mappedRequest.AddressLine2.Should().Be(_apiParameters.Location.AddressLine2);
        }

        [Test]
        public void ThenMapsAddressLine3()
        {
            _mappedRequest.AddressLine3.Should().Be(_apiParameters.Location.AddressLine3);
        }

        [Test]
        public void ThenMapsAddressLine4()
        {
            _mappedRequest.AddressLine4.Should().Be(_apiParameters.Location.AddressLine4);
        }

        [Test]
        public void ThenMapsAddressLine5()
        {
            _mappedRequest.AddressLine5.Should().Be(_apiParameters.Location.AddressLine5);
        }

        [Test]
        public void ThenMapsTown()
        {
            _mappedRequest.Town.Should().Be(_apiParameters.Location.Town);
        }

        [Test]
        public void ThenMapsPostcode()
        {
            _mappedRequest.Postcode.Should().Be(_apiParameters.Location.Postcode);
        }

        [Test]
        public void ThenMapsNumberOfPositions()
        {
            _mappedRequest.NumberOfPostions.Should().Be(_apiParameters.NumberOfPositions);
        }
    }
}
