using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class StandardRepository : IStandardRepository
    {
        private readonly IProvideSettings _provideSettings;
        private readonly ILog _logger;

        public StandardRepository(IProvideSettings provideSettings, ILog logger)
        {
            _provideSettings = provideSettings;
            _logger = logger;
        }

        private const string GetStandardAndSectorIdsQuery = "[VACANCY_API].[GetActiveStandardCodes]";

        public async Task<IEnumerable<StandardSector>> GetStandardsAndRespectiveSectorIdsAsync()
        {
            var retry = VacancyRegisterRetryPolicy.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving standard codes from database: ({exception.Message}). Retrying...attempt {retryCount}");
            });

            return await retry.ExecuteAsync(InternalGetStandardsAndRespectiveSectorIdsAsync);
        }

        private async Task<IEnumerable<StandardSector>> InternalGetStandardsAndRespectiveSectorIdsAsync()
        {
            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            using (var sqlConn = new SqlConnection(connectionString))
            {
                await sqlConn.OpenAsync();

                var results =
                    await sqlConn.QueryAsync<StandardSector>(GetStandardAndSectorIdsQuery, 
                    commandType:CommandType.StoredProcedure);

                return results;
            }
        }
    }
}
