using System;
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
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(12);
        private const string CacheKeyAllFrameworks = "VacancyApi.GetAllFrameworkDetails";
        private const string CacheKeyAllStandards = "VacancyApi.GetAllStandardDetails";

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

        public async Task<IEnumerable<TrainingDetail>> GetAllFrameworkDetailsAsync()
        {
            return await _cacheService.CacheAsideAsync(
                CacheKeyAllFrameworks,
                async () => await _trainingDetailService.GetAllFrameworkDetailsAsync(),
                _cacheDuration);
        }

        public async Task<IEnumerable<TrainingDetail>> GetAllStandardDetailsAsync()
        {
            return await _cacheService.CacheAsideAsync(
                CacheKeyAllStandards,
                async () => await _trainingDetailService.GetAllStandardDetailsAsync(),
                _cacheDuration);
        }
    }
}