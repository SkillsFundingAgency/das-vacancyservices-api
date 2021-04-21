using SFA.DAS.Http.Configuration;

namespace Esfa.Vacancy.Domain.Configuration
{
    public class CoursesApiConfiguration : IManagedIdentityClientConfiguration
    {
        public string ApiBaseUrl { get; }
        public string IdentifierUri { get; }
    }
}