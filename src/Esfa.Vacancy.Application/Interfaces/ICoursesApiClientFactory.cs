using SFA.DAS.Http;

namespace Esfa.Vacancy.Application.Interfaces
{
    public interface ICoursesApiClientFactory
    {
        IRestHttpClient CreateRestHttpClient();
    }
}