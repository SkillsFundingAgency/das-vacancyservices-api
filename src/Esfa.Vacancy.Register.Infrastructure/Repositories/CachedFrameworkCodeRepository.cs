using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Interfaces;
using Esfa.Vacancy.Register.Domain.Repositories;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class CachedFrameworkCodeRepository : IFrameworkCodeRepository
    {
        private readonly IFrameworkCodeRepository _frameworkCodeRepository;
        private readonly ILog _logger;
        private readonly ICacheService _cache;

        private const string FrameworkCodesCacheKey = "VacancyApi.FrameworkCodes";
        private const double CacheExpirationHours = 1;
        
        public CachedFrameworkCodeRepository(IFrameworkCodeRepository frameworkCodeRepository, ILog logger, ICacheService cache)
        {
            _frameworkCodeRepository = frameworkCodeRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            return await _cache.CacheAsideAsync(
                FrameworkCodesCacheKey,
                _frameworkCodeRepository.GetAsync,
                TimeSpan.FromHours(CacheExpirationHours));
        }
        
    }
}
