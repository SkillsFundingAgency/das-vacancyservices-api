using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Manage.Api.Mappings;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Api.Mappings.GivenACreateApprenticeshipRequestMapper
{
    [TestFixture]
    public class WhenMappingTitle
    {
        [Test]
        public void ShouldMapTitleToRequest()
        {
            var fixture = new Fixture();
            var parameters = fixture.Create<CreateApprenticeshipParameters>();
            var mapper = new CreateApprenticeshipRequestMapper();

            var request = mapper.MapFromApiParameters(parameters);

            request.Title.Should().Be(parameters.Title);
        }
    }
}