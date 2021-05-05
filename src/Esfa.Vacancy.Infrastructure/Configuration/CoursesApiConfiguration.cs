using SFA.DAS.Http.Configuration;

namespace Esfa.Vacancy.Infrastructure.Configuration
{
    public class CoursesApiConfiguration : IManagedIdentityClientConfiguration
    {
        public CoursesApiConfiguration(string apiBaseUrl, string identifierUri)
        {
            ApiBaseUrl = apiBaseUrl;
            IdentifierUri = identifierUri;
        }

        public string ApiBaseUrl { get; }
        public string IdentifierUri { get; }
    }
}