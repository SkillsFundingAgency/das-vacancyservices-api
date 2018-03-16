using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class CachedTrainingDetailService : ITrainingDetailService
    {
        private readonly ICacheService _cacheService;
        private readonly ITrainingDetailService _trainingDetailService;
        private const string CacheKeyAllFrameworks = "VacancyApi.GetAllFrameworkDetails";
        private const string CacheKeyAllStandards = "VacancyApi.GetAllStandardDetails";
        private const string CacheKeyFramework = "V1-Framework-{0}";

        public CachedTrainingDetailService(
            ICacheService cacheService, ITrainingDetailService trainingDetailService)
        {
            _cacheService = cacheService;
            _trainingDetailService = trainingDetailService;
        }

        public async Task<Framework> GetFrameworkDetailsAsync(int code)
        {
            return await _cacheService.CacheAsideAsync(
                                          string.Format(CacheKeyFramework, code),
                                          async () => await _trainingDetailService.GetFrameworkDetailsAsync(code)
                                                                                  .ConfigureAwait(false))
                                      .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TrainingDetail>> GetAllFrameworkDetailsAsync()
        {
            return await _cacheService.CacheAsideAsync(
                                          CacheKeyAllFrameworks,
                                          async () => await _trainingDetailService.GetAllFrameworkDetailsAsync()
                                                                                  .ConfigureAwait(false))
                                      .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TrainingDetail>> GetAllStandardDetailsAsync()
        {
            return await _cacheService.CacheAsideAsync(
                                          CacheKeyAllStandards,
                                          async () => await _trainingDetailService.GetAllStandardDetailsAsync()
                                                                                  .ConfigureAwait(false))
                                      .ConfigureAwait(false);
        }
    }
}