using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Domain.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class StandardRepository : IStandardRepository
    {
        private readonly IProvideSettings _provideSettings;
        private readonly ILog _logger;

        private const string GetActiveStandardCodesSqlSproc = "[VACANCY_API].[GetActiveStandardCodes]";

        public StandardRepository(IProvideSettings provideSettings, ILog logger)
        {
            _provideSettings = provideSettings;
            _logger = logger;
        }

        public async Task<IEnumerable<int>> GetStandardIdsAsync()
        {
            var retry = VacancyRegisterRetryPolicy.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving standard codes from database: ({exception.Message}). Retrying...attempt {retryCount}");
            });

            return await retry.ExecuteAsync(InternalGetStandardIdsAsync);
        }

        private async Task<IEnumerable<int>> InternalGetStandardIdsAsync()
        {
            _logger.Info("Querying AVMS database for Standard Codes");

            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            using (var sqlConn = new SqlConnection(connectionString))
            {
                await sqlConn.OpenAsync();

                var results =
                    await sqlConn.QueryAsync<int>(GetActiveStandardCodesSqlSproc,
                        commandType: CommandType.StoredProcedure);

                _logger.Info($"Retrieved {results.Count()} Framework Codes from AVMS database.");

                return results;
            }
        }
    }
}
