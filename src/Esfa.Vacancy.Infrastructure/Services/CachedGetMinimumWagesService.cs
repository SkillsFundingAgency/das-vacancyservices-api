using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class CachedGetMinimumWagesService : IGetMinimumWagesService
    {
        private readonly IGetMinimumWagesService _minimumWagesService;
        private readonly ILog _logger;
        private readonly ICacheService _cacheService;

        private const string CacheKey = "VacancyApi.WageRanges";

        public CachedGetMinimumWagesService(IGetMinimumWagesService minimumWagesService, ILog logger, ICacheService cacheService)
        {
            _minimumWagesService = minimumWagesService;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<WageRange>> GetAllWagesAsync()
        {
            _logger.Debug("Cache hit for GetMinimumWagesService");

            return await _cacheService.CacheAsideAsync(CacheKey, _minimumWagesService.GetAllWagesAsync);
        }

        public async Task<WageRange> GetWageRangeAsync(DateTime expectedStartDate)
        {
            var ranges = await GetAllWagesAsync();

            return ranges?.Single(range =>
                range.ValidFrom.Date <= expectedStartDate.Date &&
                range.ValidTo.Date >= expectedStartDate.Date);
        }
    }
}