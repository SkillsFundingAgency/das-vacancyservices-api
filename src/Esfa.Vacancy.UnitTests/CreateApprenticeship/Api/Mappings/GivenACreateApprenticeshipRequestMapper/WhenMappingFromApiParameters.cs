using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Manage.Api.Mappings;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

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
    }
}