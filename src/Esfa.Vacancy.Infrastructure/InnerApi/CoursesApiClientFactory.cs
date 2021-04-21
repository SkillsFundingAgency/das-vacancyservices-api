using Esfa.Vacancy.Application.Interfaces;
using SFA.DAS.Http;
using SFA.DAS.Http.Configuration;

namespace Esfa.Vacancy.Infrastructure.InnerApi
{
    public class CoursesApiClientFactory : ICoursesApiClientFactory
    {
        private readonly IManagedIdentityClientConfiguration _configuration;

        public CoursesApiClientFactory(IManagedIdentityClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IRestHttpClient CreateRestHttpClient()
        {
            var httpClient = new ManagedIdentityHttpClientFactory(_configuration).CreateHttpClient();
            httpClient.DefaultRequestHeaders.Remove("X-Version");
            httpClient.DefaultRequestHeaders.Add("X-Version", "1.0");

            var restHttpClient = new RestHttpClient(httpClient);
            return restHttpClient;
        }
    }
}