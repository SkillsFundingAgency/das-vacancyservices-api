using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.InnerApi.Responses;
using SFA.DAS.Http;

namespace Esfa.Vacancy.Infrastructure.InnerApi
{
    public class TrainingCourseService : ITrainingCourseService
    {
        private readonly IRestHttpClient _coursesApiClient;

        public TrainingCourseService(ICoursesApiClientFactory coursesApiClientFactory)
        {
            _coursesApiClient = coursesApiClientFactory.CreateRestHttpClient();
        }

        public async Task<IEnumerable<TrainingDetail>> GetStandards()
        {
            var apiResponse = await _coursesApiClient.Get<GetStandardsApiResponse>("api/courses/standards?filter=ActiveAvailable");

            return apiResponse.Standards.Select(item => (TrainingDetail)item);
        }

        public async Task<IEnumerable<TrainingDetail>> GetFrameworks()
        {
            var apiResponse = await _coursesApiClient.Get<GetFrameworksApiResponse>("api/courses/frameworks");

            return apiResponse.Frameworks.Select(item => (TrainingDetail)item);
        }
    }
}