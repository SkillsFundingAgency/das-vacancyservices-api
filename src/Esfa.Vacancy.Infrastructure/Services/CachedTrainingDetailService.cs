using System;
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
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(12);

        public CachedTrainingDetailService(
            ICacheService cacheService, ITrainingDetailService trainingDetailService)
        {
            _cacheService = cacheService;
            _trainingDetailService = trainingDetailService;
        }

        public async Task<Framework> GetFrameworkDetailsAsync(int code)
        {
            return await _cacheService.CacheAsideAsync<Framework>(
                $"V1-Framework-{code}",
                async () => await _trainingDetailService.GetFrameworkDetailsAsync(code),
                _cacheDuration);
        }

        public async Task<Standard> GetStandardDetailsAsync(int code)
        {
            return await _cacheService.CacheAsideAsync<Standard>(
                $"V1-Standard-{code}",
                async () => await _trainingDetailService.GetStandardDetailsAsync(code),
                _cacheDuration);
        }

        public async Task<TrainingDetail> GetFrameworkDetailsAsync(string frameworkCode)
        {
            return await _cacheService.CacheAsideAsync(
                $"V2-Framework-{frameworkCode}",
                async () => await _trainingDetailService.GetFrameworkDetailsAsync(frameworkCode),
                _cacheDuration);
        }

        public async Task<TrainingDetail> GetStandardDetailsAsync(string standardLarsCode)
        {
            return await _cacheService.CacheAsideAsync(
                $"V2-Standard-{standardLarsCode}",
                async () => await _trainingDetailService.GetStandardDetailsAsync(standardLarsCode),
                _cacheDuration);
        }
    }
}