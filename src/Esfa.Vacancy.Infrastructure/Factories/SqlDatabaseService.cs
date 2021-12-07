using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Factories
{
    public class SqlDatabaseService
    {
        private readonly IProvideSettings _settings;
        private readonly ILog _logger;

        public SqlDatabaseService(IProvideSettings settings, ILog logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public SqlConnection GetConnection()
        {
            var connectionString = _settings.GetSetting(ApplicationSettingKeys.AvmsPlusDatabaseConnectionStringKey);
            var environmentName = _settings.GetSetting(ApplicationSettingKeys.EnvironmentName);

            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            const string azureResource = "https://database.windows.net/";

            return (environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase) ||
                   environmentName.Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
                ? new SqlConnection(connectionString)
                : new SqlConnection
                {
                    ConnectionString = connectionString,
                    AccessToken = azureServiceTokenProvider.GetAccessTokenAsync(azureResource).Result
                };
            
        }

        public async Task<T> ExecuteWithRetryAsync<T>(string operationName, Func<SqlConnection, Task<T>> asyncFunc)
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Database operation {operationName} failed with error: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            var result = await retry.ExecuteAsync(async () =>
            {
                using (var conn = GetConnection())
                {
                    await conn.OpenAsync().ConfigureAwait(false);

                    return await asyncFunc(conn).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);

            return result;
        }
    }
}