using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class CachedGetAllApprenticeMinimumWagesService : IGetAllApprenticeMinimumWagesService
    {
        private readonly IGetAllApprenticeMinimumWagesService _minimumWagesService;
        private readonly ILog _logger;
        private readonly ICacheService _cacheService;

        private const string CacheKey = "VacancyApi.GetAllApprenticeMinimumWages";
        private const double CacheExpirationHours = 1;

        public CachedGetAllApprenticeMinimumWagesService(IGetAllApprenticeMinimumWagesService minimumWagesService, ILog logger, ICacheService cacheService)
        {
            _minimumWagesService = minimumWagesService;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<WageRange>> GetAllWagesAsync()
        {
            _logger.Debug("Cache hit for GetAllApprenticeMinimumWagesService");

            return await _cacheService.CacheAsideAsync(
                CacheKey,
                _minimumWagesService.GetAllWagesAsync,
                TimeSpan.FromHours(CacheExpirationHours));
        }
    }
}