using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class FrameworkCodeRepository : IFrameworkCodeRepository
    {
        private readonly IProvideSettings _settings;

        private const string GetFrameworkCodesQuery = @"
            SELECT 
                CodeName
            FROM 
                dbo.ApprenticeshipFramework
            WHERE 
                ApprenticeshipFrameworkStatusTypeId = 1";

        public FrameworkCodeRepository(IProvideSettings settings)
        {
            _settings = settings;
        }

        public async Task<IEnumerable<string>> GetAsync()
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