using Esfa.Vacancy.Manage.Api.Mappings;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Api.Mappings.GivenACreateApprenticeshipResponseMapper
{
    [TestFixture]
    public class WhenMappingVacancyReferenceNumber
    {
        [Test]
        public void ShouldMapVacancyReferenceToApiResponse()
        {
            var fixture = new Fixture();
            var applicationResponse = fixture.Create<CreateApprenticeshipResponse>();
            var mapper = new CreateApprenticeshipResponseMapper();

            var apiResponse = mapper.MapToApiResponse(applicationResponse);

            apiResponse.VacancyReferenceNumber.Should().Be(applicationResponse.VacancyReferenceNumber);
        }
    }
}