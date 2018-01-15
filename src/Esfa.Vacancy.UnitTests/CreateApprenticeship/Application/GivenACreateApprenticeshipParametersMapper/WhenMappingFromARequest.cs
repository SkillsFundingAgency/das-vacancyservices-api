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
        public void ThenMapsHoursPerWeek()
        {
            _mappedParameters.HoursPerWeek.Should().Be(_request.HoursPerWeek);
        }
    }
}