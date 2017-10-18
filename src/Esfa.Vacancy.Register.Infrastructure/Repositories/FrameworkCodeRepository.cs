using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class FrameworkCodeRepository : IFrameworkCodeRepository
    {
        private readonly IProvideSettings _settings;
        private readonly ILog _logger;

        private const string GetFrameworkCodesQuery = @"
            SELECT 
                CodeName
            FROM 
                dbo.ApprenticeshipFramework
            WHERE 
                ApprenticeshipFrameworkStatusTypeId = 1";

        public FrameworkCodeRepository(IProvideSettings settings, ILog logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            var retry = VacancyRegisterRetryPolicy.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving framework codes from database: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(InternalGetAsync);
        }

        private async Task<IEnumerable<string>> InternalGetAsync()
        {
            var connectionString =
                _settings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            using (var sqlConn = new SqlConnection(connectionString))
            {
                await sqlConn.OpenAsync();

                var results =
                    await sqlConn.QueryAsync<string>(GetFrameworkCodesQuery);

                return results;
            }
        }
    }
}