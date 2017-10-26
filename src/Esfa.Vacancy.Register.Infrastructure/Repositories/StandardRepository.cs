using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class StandardRepository : IStandardRepository
    {
        private readonly IProvideSettings _provideSettings;
        private readonly ILog _logger;
        private readonly ICacheService _cache;

        private const string GetActiveStandardCodesSqlSproc = "[VACANCY_API].[GetActiveStandardCodes]";
        private const string StandardCodesCacheKey = "VacancyApi.StandardCodes";
        private const double CacheExpirationHours = 1;

        public StandardRepository(IProvideSettings provideSettings, ILog logger, ICacheService cache)
        {
            _provideSettings = provideSettings;
            _logger = logger;
            _cache = cache;
        }

        private const string GetActiveStandardCodesSqlSproc = "[VACANCY_API].[GetActiveStandardCodes]";

        public async Task<IEnumerable<int>> GetStandardIdsAsync()
        {
            var retry = VacancyRegisterRetryPolicy.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving standard codes from database: ({exception.Message}). Retrying...attempt {retryCount}");
            });

            return await retry.ExecuteAsync(() => _cache.CacheAsideAsync(
                StandardCodesCacheKey, 
                InternalGetStandardsAndRespectiveSectorIdsAsync, 
                TimeSpan.FromHours(CacheExpirationHours)));
        }

        private async Task<IEnumerable<int>> InternalGetStandardIdsAsync()
        {
            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            using (var sqlConn = new SqlConnection(connectionString))
            {
                await sqlConn.OpenAsync();

                var results =
                    await sqlConn.QueryAsync<int>(GetActiveStandardCodesSqlSproc,
                        commandType: CommandType.StoredProcedure);

                return results;
            }
        }
    }
}
