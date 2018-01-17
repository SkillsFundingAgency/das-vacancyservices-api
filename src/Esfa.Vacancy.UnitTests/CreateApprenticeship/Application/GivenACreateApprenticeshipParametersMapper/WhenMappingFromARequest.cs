using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingFromARequest
    {
        private CreateApprenticeshipParameters _mappedParameters;
        private CreateApprenticeshipRequest _request;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();
            _request = fixture.Create<CreateApprenticeshipRequest>();

            var mapper = fixture.Create<CreateApprenticeshipParametersMapper>();

            _mappedParameters = mapper.MapFromRequest(_request);
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
        public void ThenMapsLocationType()
        {
            _mappedParameters.LocationTypeId.Should().Be(1);
        }

        [Test]
        public void ThenMapsAddressLine1()
        {
            _mappedParameters.AddressLine1.Should().Be(_request.AddressLine1);
        }

        [Test]
        public void ThenMapsAddressLine2()
        {
            _mappedParameters.AddressLine2.Should().Be(_request.AddressLine2);
        }

        [Test]
        public void ThenMapsAddressLine3()
        {
            _mappedParameters.AddressLine3.Should().Be(_request.AddressLine3);
        }

        [Test]
        public void ThenMapsAddressLine4()
        {
            _mappedParameters.AddressLine4.Should().Be(_request.AddressLine4);
        }

        [Test]
        public void ThenMapsAddressLine5()
        {
            _mappedParameters.AddressLine5.Should().Be(_request.AddressLine5);
        }

        [Test]
        public void ThenMapsTown()
        {
            _mappedParameters.Town.Should().Be(_request.Town);
        }

        [Test]
        public void ThenMapsPostcode()
        {
            _mappedParameters.Postcode.Should().Be(_request.Postcode);
        }
    }
}