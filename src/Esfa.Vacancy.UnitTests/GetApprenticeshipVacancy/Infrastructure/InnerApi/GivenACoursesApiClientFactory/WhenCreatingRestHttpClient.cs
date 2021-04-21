using Esfa.Vacancy.Infrastructure.InnerApi;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Http;
using SFA.DAS.Http.Configuration;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Infrastructure.InnerApi.GivenACoursesApiClientFactory
{
    public class WhenCreatingRestHttpClient
    {
        [Test]
        public void Then_Returns_RestHttpClient()
        {
            var mockConfig = new Mock<IManagedIdentityClientConfiguration>();
            mockConfig
                .SetupGet(configuration => configuration.ApiBaseUrl)
                .Returns("https://lost.somewhere/notlost");
            mockConfig
                .SetupGet(configuration => configuration.IdentifierUri)
                .Returns("https://iamlegit.com/maybe");

            var factory = new CoursesApiClientFactory(mockConfig.Object);

            var client = factory.CreateRestHttpClient();
            client.Should().NotBeNull();
            client.Should().BeAssignableTo<IRestHttpClient>();
        }
    }
}