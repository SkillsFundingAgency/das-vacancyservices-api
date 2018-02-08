using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Domain.Repositories;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Repositories
{
    public class FrameworkCodeRepository : IFrameworkCodeRepository
    {
        private readonly IProvideSettings _settings;
        private readonly ILog _logger;

        private const string GetActiveFrameworkCodesSqlSproc = "[VACANCY_API].[GetActiveFrameworkCodes]";

        public FrameworkCodeRepository(IProvideSettings settings, ILog logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving framework codes from database: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(InternalGetAsync);
        }

        private async Task<IEnumerable<string>> InternalGetAsync()
        {
            _logger.Info("Querying AVMS database for Framework Codes");
            var connectionString =
                _settings.GetSetting(ApplicationSettingKeys.AvmsPlusDatabaseConnectionStringKey);

            using (var sqlConn = new SqlConnection(connectionString))
            {
                await sqlConn.OpenAsync();

                var results =
                    await sqlConn.QueryAsync<string>(GetActiveFrameworkCodesSqlSproc, commandType: CommandType.StoredProcedure);


                _logger.Info($"Retrieved {results.Count()} Framework Codes from AVMS database.");

                return results;
            }
        }
    }
}