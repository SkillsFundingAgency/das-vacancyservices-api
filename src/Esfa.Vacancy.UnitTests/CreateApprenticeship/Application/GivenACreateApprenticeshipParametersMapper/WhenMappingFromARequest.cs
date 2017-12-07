using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingFromARequest
    {
        [Test]
        public void ThenMapsTitle()
        {
            var fixture = new Fixture();
            var request = fixture.Create<CreateApprenticeshipRequest>();

            var mapper = fixture.Create<CreateApprenticeshipParametersMapper>();

            var parameters = mapper.MapFromRequest(request);

            parameters.Title.Should().Be(request.Title);
        }
    }
}