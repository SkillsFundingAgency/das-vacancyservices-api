using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Interfaces;
using Esfa.Vacancy.Register.Domain.Repositories;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class CachedStandardRepository : IStandardRepository
    {
        private readonly IStandardRepository _standardRepository;
        private readonly ILog _logger;
        private readonly ICacheService _cache;

        private const string StandardCodesCacheKey = "VacancyApi.StandardCodes";
        private const double CacheExpirationHours = 1;

        public CachedStandardRepository(IStandardRepository standardRepository, ILog logger, ICacheService cache)
        {
            _standardRepository = standardRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<int>> GetStandardIdsAsync()
        {
            var retry = VacancyRegisterRetryPolicy.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error caching standard codes: ({exception.Message}). Retrying...attempt {retryCount}");
            });

            return await retry.ExecuteAsync(() => _cache.CacheAsideAsync(
                StandardCodesCacheKey,
                _standardRepository.GetStandardIdsAsync, 
                TimeSpan.FromHours(CacheExpirationHours)));
        }
        
    }
}
