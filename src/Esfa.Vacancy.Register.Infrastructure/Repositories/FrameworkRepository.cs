using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class FrameworkRepository : IFrameworkRepository
    {
        private readonly IProvideSettings _settings;

        private const string GetFrameworksQuery = @"
            SELECT 
                CodeName
            FROM 
                dbo.ApprenticeshipFramework
            WHERE 
                ApprenticeshipFrameworkStatusTypeId = 1";

        public FrameworkRepository(IProvideSettings settings)
        {
            _settings = settings;
        }

        public async Task<IEnumerable<string>> GetFrameworksAsync()
        {
            var connectionString =
                _settings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            using (var sqlConn = new SqlConnection(connectionString))
            {
                await sqlConn.OpenAsync();

                var results =
                    await sqlConn.QueryAsync<string>(GetFrameworksQuery);

                return results;
            }
        }
    }
}